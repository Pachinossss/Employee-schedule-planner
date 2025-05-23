using EmployeeSchedulePlanner.Forms;
using System;
using System.Windows.Forms;

namespace EmployeeSchedulePlanner
{
    static class Program
    {
        [STAThread]
// Programmas sākuma punkts – palaiž galveno logu
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
// Palaiž sākotnējo logu
            Application.Run(new LoginForm());

        }
    }
}