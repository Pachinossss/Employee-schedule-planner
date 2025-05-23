using EmployeeSchedulePlanner.Data;
using EmployeeSchedulePlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeSchedulePlanner.Forms
{
    // Logs, kurā lietotājs var veidot, labot un dzēst projektus
    public partial class ProjectForm : Form
    {
        private int? selectedProjectId = null;

        public ProjectForm()
        {
            InitializeComponent();
            // Ielādē projektus no datubāzes un attēlo sarakstā
            LoadProjects();
            // Ielādē visus pieejamos darbiniekus, kurus var pievienot projektam
            LoadAvailableEmployees();
        }

        // Ielādē projektus no datubāzes un attēlo sarakstā
        private void LoadProjects()
        {
            projectsListBox.Items.Clear();
            using var db = new ScheduleDbContext();
            foreach (var project in db.Projects.ToList())
            {
                projectsListBox.Items.Add($"{project.Id}: {project.Name} ({project.StartDate:yyyy-MM-dd} - {project.EndDate:yyyy-MM-dd})");
            }
        }

        // Ielādē visus pieejamos darbiniekus, kurus var pievienot projektam
        private void LoadAvailableEmployees()
        {
            availableEmployeesListBox.Items.Clear();
            using var db = new ScheduleDbContext();
            foreach (var user in db.Users.ToList())
            {
                availableEmployeesListBox.Items.Add(new UserDisplayWrapper(user));
            }
        }

        // Notīra ievades laukus un atiestata formas sākotnējo stāvokli
        private void ClearForm()
        {
            projectNameTxtBox.Text = "";
            startDatePicker.Value = DateTime.Today;
            endDatePicker.Value = DateTime.Today;
            assignedEmployeesListBox.Items.Clear();
            selectedProjectId = null;
        }

        // Pārbauda vai darbinieks nav aizņemts citā projektā šajā periodā
        private bool IsEmployeeAvailable(int userId, DateTime start, DateTime end, int? currentProjectId = null)
        {
            using var db = new ScheduleDbContext();
            return !db.EmployeeProjects
                      .Where(ep => ep.UserId == userId && (currentProjectId == null || ep.ProjectId != currentProjectId))
                      .Any(ep => db.Projects
                                    .Where(p => p.Id == ep.ProjectId)
                                    .Any(p => (start <= p.EndDate && end >= p.StartDate)));
        }

        // Saglabā jaunu projektu un pievieno izvēlētos darbiniekus, ja tie ir pieejami
        private void saveProjectBtn_Click(object sender, EventArgs e)
        {
            string name = projectNameTxtBox.Text.Trim();
            DateTime start = startDatePicker.Value;
            DateTime end = endDatePicker.Value;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Lūdzu ievadi projekta nosaukumu.");
                return;
            }

            if (end < start)
            {
                MessageBox.Show("Beigu datums nevar būt pirms sākuma datuma.");
                return;
            }

            using var db = new ScheduleDbContext();
            var project = new Project
            {
                Name = name,
                StartDate = start,
                EndDate = end,
                Description = "",
                EmployeeProjects = new List<EmployeeProject>()
            };

            // Pārskatām visus pievienotos darbiniekus un pārbaudām pieejamību
            foreach (UserDisplayWrapper wrapper in assignedEmployeesListBox.Items)
            {
                var user = wrapper.User;
                // Pārbauda vai darbinieks nav aizņemts citā projektā šajā periodā
                if (!IsEmployeeAvailable(user.Id, start, end))
                {
                    MessageBox.Show($"Lietotājs {user.Username} jau ir aizņemts šajā laika periodā.", "Konflikts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Pievieno darbinieku projektam
                project.EmployeeProjects.Add(new EmployeeProject { UserId = user.Id });
            }

            db.Projects.Add(project);
            // Saglabā izmaiņas datubāzē
            db.SaveChanges();
            // Ielādē projektus no datubāzes un attēlo sarakstā
            LoadProjects();
            // Notīra ievades laukus un atiestata formas sākotnējo stāvokli
            ClearForm();
        }

        // Pārceļ darbinieku no pieejamajiem uz pievienotajiem sarakstu
        private void assignEmployeeBtn_Click(object sender, EventArgs e)
        {
            if (availableEmployeesListBox.SelectedItem is UserDisplayWrapper selected)
            {
                assignedEmployeesListBox.Items.Add(selected);
                availableEmployeesListBox.Items.Remove(selected);
            }
        }

        // Kad lietotājs izvēlas projektu sarakstā, forma aizpildās ar šī projekta datiem
        private void projectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (projectsListBox.SelectedItem == null) return;
            // Formatē darbinieka informāciju skatam sarakstā
            var id = int.Parse(projectsListBox.SelectedItem.ToString().Split(':')[0]);

            using var db = new ScheduleDbContext();
            var project = db.Projects
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (project != null)
            {
                selectedProjectId = project.Id;
                projectNameTxtBox.Text = project.Name;
                startDatePicker.Value = project.StartDate;
                endDatePicker.Value = project.EndDate;

                assignedEmployeesListBox.Items.Clear();
                availableEmployeesListBox.Items.Clear();

                var assignedIds = db.EmployeeProjects
                    .Where(ep => ep.ProjectId == project.Id)
                    .Select(ep => ep.UserId)
                    .ToHashSet();

                foreach (var user in db.Users.ToList())
                {
                    var wrapper = new UserDisplayWrapper(user);
                    if (assignedIds.Contains(user.Id))
                        assignedEmployeesListBox.Items.Add(wrapper);
                    else
                        availableEmployeesListBox.Items.Add(wrapper);
                }
            }
        }

        // Atjaunina projekta informāciju un darbinieku sarakstu
        private void updateProjectBtn_Click(object sender, EventArgs e)
        {
            if (selectedProjectId == null) return;

            using var db = new ScheduleDbContext();
            var project = db.Projects.Find(selectedProjectId);
            if (project != null)
            {
                var start = startDatePicker.Value;
                var end = endDatePicker.Value;

                project.Name = projectNameTxtBox.Text.Trim();
                project.StartDate = start;
                project.EndDate = end;

                var existing = db.EmployeeProjects.Where(ep => ep.ProjectId == project.Id).ToList();
                db.EmployeeProjects.RemoveRange(existing);

                // Pārskatām visus pievienotos darbiniekus un pārbaudām pieejamību
                foreach (UserDisplayWrapper wrapper in assignedEmployeesListBox.Items)
                {
                    var user = wrapper.User;
                    // Pārbauda vai darbinieks nav aizņemts citā projektā šajā periodā
                    if (!IsEmployeeAvailable(user.Id, start, end, project.Id))
                    {
                        MessageBox.Show($"Lietotājs {user.Username} jau ir aizņemts šajā laika periodā.", "Konflikts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    db.EmployeeProjects.Add(new EmployeeProject { ProjectId = project.Id, UserId = user.Id });
                }

                // Saglabā izmaiņas datubāzē
                db.SaveChanges();
                // Ielādē projektus no datubāzes un attēlo sarakstā
                LoadProjects();
                // Notīra ievades laukus un atiestata formas sākotnējo stāvokli
                ClearForm();
            }
        }

        // Dzēš izvēlēto projektu un tam piesaistītos darbiniekus
        private void deleteProjectBtn_Click(object sender, EventArgs e)
        {
            if (selectedProjectId == null) return;

            using var db = new ScheduleDbContext();
            var project = db.Projects.Find(selectedProjectId);
            if (project != null)
            {
                var related = db.EmployeeProjects.Where(ep => ep.ProjectId == project.Id).ToList();
                db.EmployeeProjects.RemoveRange(related);
                db.Projects.Remove(project);
                // Saglabā izmaiņas datubāzē
                db.SaveChanges();
                // Ielādē projektus no datubāzes un attēlo sarakstā
                LoadProjects();
                // Notīra ievades laukus un atiestata formas sākotnējo stāvokli
                ClearForm();
            }
        }
    }

    // Palīgklase, lai viegli attēlotu lietotājus sarakstos
    public class UserDisplayWrapper
    {
        public User User { get; }

        public UserDisplayWrapper(User user)
        {
            User = user;
        }

        // Formatē darbinieka informāciju skatam sarakstā
        public override string ToString()
        {
            return $"{User.Name} {User.Surname} ({User.Role})";
        }
    }
}