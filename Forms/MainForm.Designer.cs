using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EmployeeSchedulePlanner.Forms
{
    partial class MainForm
    {
        private Guna2Button manageEmployeesBtn;
        private Guna2Button manageProjectsBtn;
        private Label titleLabel;

        private void InitializeComponent()
        {
            this.manageEmployeesBtn = new Guna2Button();
            this.manageProjectsBtn = new Guna2Button();
            this.titleLabel = new Label();

            // Form settings
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ClientSize = new Size(400, 300);
            this.Text = "Galvenā izvēlne";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title label
            this.titleLabel.Text = "Darbinieku un projektu pārvaldība";
            this.titleLabel.ForeColor = Color.White;
            this.titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new Point(50, 30);

            // Employees button
            this.manageEmployeesBtn.Text = "Pārvaldīt darbiniekus";
            this.manageEmployeesBtn.Size = new Size(250, 45);
            this.manageEmployeesBtn.FillColor = Color.FromArgb(0, 120, 215);
            this.manageEmployeesBtn.ForeColor = Color.White;
            this.manageEmployeesBtn.Location = new Point(75, 90);
            this.manageEmployeesBtn.Click += (s, e) => new EmployeeForm().ShowDialog();

            // Projects button
            this.manageProjectsBtn.Text = "Pārvaldīt projektus";
            this.manageProjectsBtn.Size = new Size(250, 45);
            this.manageProjectsBtn.FillColor = Color.FromArgb(0, 153, 51);
            this.manageProjectsBtn.ForeColor = Color.White;
            this.manageProjectsBtn.Location = new Point(75, 150);
            this.manageProjectsBtn.Click += (s, e) => new ProjectForm().ShowDialog();

            // Add controls
            this.Controls.Add(titleLabel);
            this.Controls.Add(manageEmployeesBtn);
            this.Controls.Add(manageProjectsBtn);
        }
    }
}
