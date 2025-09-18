//using Microsoft.EntityFrameworkCore;
//using TaskBoardApi.Models.Domain;
//using System;

//namespace TaskBoardApi.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public DbSet<User> Users { get; set; }
//        public DbSet<Role> Roles { get; set; }
//        public DbSet<UserProfile> UserProfiles { get; set; }
//        public DbSet<Project> Projects { get; set; }
//        public DbSet<ProjectMember> ProjectMembers { get; set; }
//        public DbSet<TaskItem> TaskItems { get; set; }
//        public DbSet<TaskAssignment> TaskAssignments { get; set; }
//        public DbSet<Tag> Tags { get; set; }
//        public DbSet<TaskTag> TaskTags { get; set; }

//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // --- Fluent API configurations (your existing code) ---
//            modelBuilder.Entity<User>()
//                .HasOne(u => u.Role)
//                .WithMany(r => r.Users)
//                .HasForeignKey(u => u.RoleId);

//            modelBuilder.Entity<UserProfile>()
//                .HasKey(u => u.UserId);

//            modelBuilder.Entity<UserProfile>()
//                .HasOne(p => p.User)
//                .WithOne(u => u.Profile)
//                .HasForeignKey<UserProfile>(p => p.UserId);

//            modelBuilder.Entity<Project>()
//                .HasOne(p => p.Owner)
//                .WithMany(u => u.Projects)
//                .HasForeignKey(p => p.OwnerId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<ProjectMember>()
//                .HasKey(pm => new { pm.ProjectId, pm.UserId });
//            modelBuilder.Entity<ProjectMember>()
//                .HasOne(pm => pm.Project)
//                .WithMany(p => p.Members)
//                .HasForeignKey(pm => pm.ProjectId);
//            modelBuilder.Entity<ProjectMember>()
//                .HasOne(pm => pm.User)
//                .WithMany()
//                .HasForeignKey(pm => pm.UserId);

//            modelBuilder.Entity<TaskItem>()
//                .HasOne(t => t.Project)
//                .WithMany(p => p.Tasks)
//                .HasForeignKey(t => t.ProjectId);

//            modelBuilder.Entity<TaskItem>()
//                .HasOne(t => t.AssignedToUser)
//                .WithMany(u => u.Tasks)
//                .HasForeignKey(t => t.AssignedToUserId)
//                .OnDelete(DeleteBehavior.SetNull);

//            modelBuilder.Entity<TaskAssignment>()
//                .HasOne(a => a.TaskItem)
//                .WithMany(t => t.Assignments)
//                .HasForeignKey(a => a.TaskItemId);

//            modelBuilder.Entity<TaskAssignment>()
//                .HasOne(a => a.AssignedToUser)
//                .WithMany(u => u.Assignments)
//                .HasForeignKey(a => a.AssignedToUserId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<TaskAssignment>()
//                .HasOne(a => a.AssignedByUser)
//                .WithMany()
//                .HasForeignKey(a => a.AssignedByUserId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<TaskTag>()
//                .HasKey(tt => new { tt.TaskItemId, tt.TagId });
//            modelBuilder.Entity<TaskTag>()
//                .HasOne(tt => tt.TaskItem)
//                .WithMany(t => t.TaskTags)
//                .HasForeignKey(tt => tt.TaskItemId);
//            modelBuilder.Entity<TaskTag>()
//                .HasOne(tt => tt.Tag)
//                .WithMany(t => t.TaskTags)
//                .HasForeignKey(tt => tt.TagId);

//            // -------------------------
//            // Data Seeding
//            // -------------------------

//            // Roles
//            modelBuilder.Entity<Role>().HasData(
//                new Role { Id = 1, Name = "Admin" },
//                new Role { Id = 2, Name = "Manager" },
//                new Role { Id = 3, Name = "Developer" }
//            );

//        }
//    }
//}
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Fluent API configurations ---
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<UserProfile>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UserProfile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.UserId });
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId);
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany()
                .HasForeignKey(pm => pm.UserId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(a => a.TaskItem)
                .WithMany(t => t.Assignments)
                .HasForeignKey(a => a.TaskItemId);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(a => a.AssignedToUser)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(a => a.AssignedByUser)
                .WithMany()
                .HasForeignKey(a => a.AssignedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskTag>()
                .HasKey(tt => new { tt.TaskItemId, tt.TagId });
            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.TaskItem)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskItemId);
            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId);

            // -------------------------
            // Shadow Properties for Auditing (only for selected entities)
            // -------------------------
            Type[] auditableEntities = new Type[]
            {
                typeof(User),
                typeof(Project),
                typeof(TaskItem),
                typeof(TaskAssignment)
            };

            foreach (var entityType in auditableEntities)
            {
                modelBuilder.Entity(entityType).Property<string>("CreatedBy").HasDefaultValue(null);
                modelBuilder.Entity(entityType).Property<DateTime?>("CreatedAt").HasDefaultValue(null);
                modelBuilder.Entity(entityType).Property<string>("ModifiedBy").HasDefaultValue(null);
                modelBuilder.Entity(entityType).Property<DateTime?>("ModifiedAt").HasDefaultValue(null);
            }

            // -------------------------
            // Data Seeding
            // -------------------------
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Manager" },
                new Role { Id = 3, Name = "Developer" }
            );
        }

        public override int SaveChanges()
        {
            ApplyAuditing();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditing();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditing()
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            string? userId = null;

            if (user != null)
            {
                // Log all claims for debugging
                foreach (var claim in user.Claims)
                {
                    Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");
                }

                // First try "sub" (JwtRegisteredClaimNames.Sub)
                userId = user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

                // Fallback to NameIdentifier
                if (string.IsNullOrEmpty(userId))
                {
                    userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
            }


            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Metadata.FindProperty("CreatedBy") != null)
                        entry.Property("CreatedBy").CurrentValue = userId;

                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                        entry.Property("CreatedAt").CurrentValue = now;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry.Metadata.FindProperty("ModifiedBy") != null)
                        entry.Property("ModifiedBy").CurrentValue = userId;

                    if (entry.Metadata.FindProperty("ModifiedAt") != null)
                        entry.Property("ModifiedAt").CurrentValue = now;
                }
            }
        }
    }
}

