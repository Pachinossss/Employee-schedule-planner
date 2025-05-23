using System;

namespace EmployeeSchedulePlanner.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Aizvietots EmployeeId
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Notes { get; set; }

        public User User { get; set; }  // Aizvietots Employee
    }
}