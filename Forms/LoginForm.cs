using System;
using System.Linq;
using System.Windows.Forms;
using EmployeeSchedulePlanner.Data;
using EmployeeSchedulePlanner.Utils;
using EmployeeSchedulePlanner.Models;

namespace EmployeeSchedulePlanner.Forms
{
// Parādās user logs
    public partial class LoginForm : Form
    {
    
        private void loginBtn_Click(object sender, EventArgs e)
        {
            var username = usernameTxtBox.Text.Trim();
            var password = passwordTxtBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lūdzu ievadi gan lietotājvārdu, gan paroli.");
                return;
            }

            using var db = new ScheduleDbContext();
// Šifrē parole pirms saglabāšanas
            var hash = PasswordHasher.Hash(password);

// Šeit tiek atlasīts konkrēts lietotājs datubāzē
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);
            if (user == null)
            {
                MessageBox.Show("Nepareizs lietotājvārds vai parole!");
                return;
            }

            Hide();
            var mainForm = new MainForm(user);
            mainForm.FormClosed += (s, args) => this.Close(); // pareiza aizvēršana
            mainForm.Show();
        }
    }
}