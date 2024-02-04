using Microsoft.EntityFrameworkCore;
using Puppy_Project.Depandancies;
using Puppy_Project.Models;

namespace Puppy_Project.Dbcontext
{
    public class PuppyDb:DbContext
    {
        private readonly string _connection_string;
        public PuppyDb(IConfiguration configuration)
        {
            _connection_string = configuration["ConnectionStrings:DefaultConnection"];
        }

        public DbSet<UserDTO> UsersTb { get; set; }
        public DbSet<CategoryDTO> CategoryTB { get; set; }
        public DbSet<ProductDTO> ProductsTb { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDTO>()
                .Property(u=>u.Role)
                .HasDefaultValue("user");
            modelBuilder.Entity<ProductDTO>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.Category_id);
            modelBuilder.Entity<ProductDTO>()
                .Property(p => p.Qty)
                .HasDefaultValue(1);
        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connection_string);
            }
        }

    }
}
