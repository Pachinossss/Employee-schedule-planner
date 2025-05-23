using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EmployeeSchedulePlanner.Forms
{
    public partial class LoginForm : Form
    {
        private Guna2TextBox usernameTxtBox;
        private Guna2TextBox passwordTxtBox;
        private Guna2Button loginBtn;
        private Label usernameLabel;
        private Label passwordLabel;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.usernameTxtBox = new Guna2TextBox();
            this.passwordTxtBox = new Guna2TextBox();
            this.loginBtn = new Guna2Button();
            this.usernameLabel = new Label();
            this.passwordLabel = new Label();

            // Form settings
            this.Text = "Pieteikšanās";
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ClientSize = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Username Label
            usernameLabel.Text = "Lietotājvārds:";
            usernameLabel.ForeColor = Color.White;
            usernameLabel.Location = new Point(30, 30);
            usernameLabel.AutoSize = true;

            // Username TextBox
            usernameTxtBox.Location = new Point(140, 25);
            usernameTxtBox.Size = new Size(160, 30);
            usernameTxtBox.PlaceholderText = "Ievadi lietotājvārdu šeit";

            // Password Label
            passwordLabel.Text = "Parole:";
            passwordLabel.ForeColor = Color.White;
            passwordLabel.Location = new Point(30, 75);
            passwordLabel.AutoSize = true;

            // Password TextBox
            passwordTxtBox.Location = new Point(140, 70);
            passwordTxtBox.Size = new Size(160, 30);
            passwordTxtBox.PasswordChar = '•';
            passwordTxtBox.PlaceholderText = "Ievadi paroli šeit";

            // Login Button
            loginBtn.Text = "Ielogoties";
            loginBtn.Size = new Size(160, 35);
            loginBtn.Location = new Point(140, 120);
            loginBtn.FillColor = Color.FromArgb(0, 120, 215);
            loginBtn.ForeColor = Color.White;
            loginBtn.Click += loginBtn_Click;

            // Add controls
            this.Controls.Add(usernameLabel);
            this.Controls.Add(usernameTxtBox);
            this.Controls.Add(passwordLabel);
            this.Controls.Add(passwordTxtBox);
            this.Controls.Add(loginBtn);
        }

        // loginBtn_Click metode jau pieejama pie tevis
    }
}
