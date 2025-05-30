using System;
using System.Collections.Generic;

namespace EmployeeSchedulePlanner.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}