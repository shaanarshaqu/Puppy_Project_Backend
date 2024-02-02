using Microsoft.EntityFrameworkCore;
using Puppy_Project.Depandancies;

namespace Puppy_Project.Dbcontext
{
    public class PuppyDb:DbContext
    {
        private readonly string _connection_string;
        public DbSet<UserDTO> UsersTb { get; set; }
        

        public PuppyDb(IConfiguration configuration)
        {
            _connection_string = configuration["ConnectionStrings:DefaultConnection"];
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
