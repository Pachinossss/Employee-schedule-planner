using System.Linq;
using EmployeeSchedulePlanner.Data;
using EmployeeSchedulePlanner.Models;
using EmployeeSchedulePlanner.Utils;

namespace EmployeeSchedulePlanner.Services
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            using var db = new ScheduleDbContext();

            if (db.Users.Any(u => u.Username == "admin"))
                return;

            var adminUser = new User
            {
                Username = "admin",
// Šifrē parole pirms saglabāšanas
                PasswordHash = PasswordHasher.Hash("admin123"),
                Role = "BusinessManager",
                Name = "Admin",
                Surname = "Lietotājs",
                Email = "admin@example.com"
            };

// Šī koda daļa atbild par jauna lietotāja izveidi
            db.Users.Add(adminUser);
// Saglabā izmaiņas datubāzē
            db.SaveChanges();
        }
    }
}