using System;
using System.Windows.Forms;
using System.Drawing;
using Guna.UI2.WinForms;

namespace EmployeeSchedulePlanner.Forms
{
    partial class ProjectForm
    {
        private Guna2TextBox projectNameTxtBox;
        private Guna2DateTimePicker startDatePicker;
        private Guna2DateTimePicker endDatePicker;
        private ListBox availableEmployeesListBox;
        private ListBox assignedEmployeesListBox;
        private ListBox projectsListBox;
        private Guna2Button assignEmployeeBtn;
        private Guna2Button saveProjectBtn;
        private Guna2Button updateProjectBtn;
        private Guna2Button deleteProjectBtn;

        private void InitializeComponent()
        {
            this.projectNameTxtBox = new Guna2TextBox();
            this.startDatePicker = new Guna2DateTimePicker();
            this.endDatePicker = new Guna2DateTimePicker();
            this.availableEmployeesListBox = new ListBox();
            this.assignedEmployeesListBox = new ListBox();
            this.projectsListBox = new ListBox();
            this.assignEmployeeBtn = new Guna2Button();
            this.saveProjectBtn = new Guna2Button();
            this.updateProjectBtn = new Guna2Button();
            this.deleteProjectBtn = new Guna2Button();

            Font labelFont = new Font("Segoe UI", 10);
            Color labelColor = Color.White;

            this.SuspendLayout();
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ClientSize = new Size(850, 500);
            this.Text = "Projektu pārvaldība";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Virsraksts
            var titleLabel = new Label()
            {
                Text = "Projektu pārvaldība",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 10),
                AutoSize = true
            };
            this.Controls.Add(titleLabel);

            // Labels un komponentes
            var lblProjectName = CreateLabel("Projekta nosaukums:", 30, 60);
            projectNameTxtBox.Location = new Point(200, 55);
            projectNameTxtBox.Size = new Size(250, 36);

            var lblStart = CreateLabel("Sākuma datums:", 30, 110);
            startDatePicker.Location = new Point(200, 105);
            startDatePicker.Size = new Size(250, 36);

            var lblEnd = CreateLabel("Beigu datums:", 30, 160);
            endDatePicker.Location = new Point(200, 155);
            endDatePicker.Size = new Size(250, 36);

            var lblAvailable = CreateLabel("Pieejamie darbinieki:", 30, 210);
            availableEmployeesListBox.Location = new Point(30, 240);
            availableEmployeesListBox.Size = new Size(200, 120);
            availableEmployeesListBox.BackColor = Color.FromArgb(45, 45, 45);
            availableEmployeesListBox.ForeColor = Color.White;

            assignEmployeeBtn.Text = "→";
            assignEmployeeBtn.Size = new Size(50, 30);
            assignEmployeeBtn.Location = new Point(240, 280);
            assignEmployeeBtn.Click += assignEmployeeBtn_Click;

            var lblAssigned = CreateLabel("Pievienotie darbinieki:", 310, 210);
            assignedEmployeesListBox.Location = new Point(310, 240);
            assignedEmployeesListBox.Size = new Size(200, 120);
            assignedEmployeesListBox.BackColor = Color.FromArgb(45, 45, 45);
            assignedEmployeesListBox.ForeColor = Color.White;

            var lblProjects = CreateLabel("Projekti:", 550, 60);
            projectsListBox.Location = new Point(550, 90);
            projectsListBox.Size = new Size(250, 270);
            projectsListBox.BackColor = Color.FromArgb(45, 45, 45);
            projectsListBox.ForeColor = Color.White;
            projectsListBox.SelectedIndexChanged += projectsListBox_SelectedIndexChanged;

            saveProjectBtn = CreateButton("Saglabāt", 30, 380, Color.FromArgb(0, 120, 215));
            saveProjectBtn.Click += saveProjectBtn_Click;

            updateProjectBtn = CreateButton("Labot", 140, 380, Color.FromArgb(255, 140, 0));
            updateProjectBtn.Click += updateProjectBtn_Click;

            deleteProjectBtn = CreateButton("Dzēst", 250, 380, Color.FromArgb(200, 40, 40));
            deleteProjectBtn.Click += deleteProjectBtn_Click;

            // Pievieno kontroles
            this.Controls.AddRange(new Control[]
            {
                lblProjectName, projectNameTxtBox,
                lblStart, startDatePicker,
                lblEnd, endDatePicker,
                lblAvailable, availableEmployeesListBox,
                assignEmployeeBtn,
                lblAssigned, assignedEmployeesListBox,
                lblProjects, projectsListBox,
                saveProjectBtn, updateProjectBtn, deleteProjectBtn
            });

            this.ResumeLayout(false);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = true
            };
        }

        private Guna2Button CreateButton(string text, int x, int y, Color color)
        {
            return new Guna2Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(100, 40),
                FillColor = color,
                ForeColor = Color.White,
                BorderRadius = 6
            };
        }
    }
}
