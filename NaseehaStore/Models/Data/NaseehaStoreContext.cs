using Microsoft.EntityFrameworkCore;

namespace NaseehaStore.Models.Data
{
    public class NaseehaStoreContext : DbContext
    {
        public NaseehaStoreContext(DbContextOptions<NaseehaStoreContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Course entity to have a primary key (Id)
            modelBuilder.Entity<Course>().HasKey(c => c.Id);

            // Seed initial admin user
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                Id = 1,
                Email = "admin@naseeha.com",
                Password = "Admin@123"
            });
        }
    }
}
