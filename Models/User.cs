using EmployeeSchedulePlanner.Models;
using System.Collections.Generic;

namespace EmployeeSchedulePlanner.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}