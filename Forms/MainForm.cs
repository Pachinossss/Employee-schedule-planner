using System;
using System.Windows.Forms;
using EmployeeSchedulePlanner.Models;

namespace EmployeeSchedulePlanner.Forms
{
// Parādās user logs
    public partial class MainForm : Form
    {
        private readonly User loggedInUser;

        public MainForm(User user)
        {
            loggedInUser = user;
            InitializeComponent();

            manageEmployeesBtn.Visible = (loggedInUser.Role == "BusinessManager");
            manageProjectsBtn.Visible = (loggedInUser.Role == "BusinessManager");
        }
    }
}