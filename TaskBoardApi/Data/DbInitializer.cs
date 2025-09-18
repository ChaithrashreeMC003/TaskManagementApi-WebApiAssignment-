
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        // Apply migrations
        context.Database.Migrate();

        // --- Seed Roles ---
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Id = 1, Name = "SuperAdmin" },
                new Role { Id = 2, Name = "Admin" },
                new Role { Id = 3, Name = "Manager" },
                new Role { Id = 4, Name = "Developer" }
            );
            context.SaveChanges();
        }

        // --- Seed SuperAdmin User ---
        if (!context.Users.Any(u => u.Email == "superadmin@example.com"))
        {
            var superAdminId = Guid.NewGuid();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin123!"); // strong password

            var superAdmin = new User
            {
                Id = superAdminId,
                Email = "superadmin@example.com",
                PasswordHash = passwordHash,
                RoleId = 1 // SuperAdmin
            };
            context.Users.Add(superAdmin);

            context.UserProfiles.Add(new UserProfile
            {
                UserId = superAdminId,
                FullName = "Super Admin",
                Phone = "9999999999",
                Address = "Head Office"
            });

            context.SaveChanges();
            Console.WriteLine("✅ SuperAdmin user seeded.");
        }
        else
        {
            Console.WriteLine("ℹ️ SuperAdmin already exists, skipping.");
        }
    }
}

