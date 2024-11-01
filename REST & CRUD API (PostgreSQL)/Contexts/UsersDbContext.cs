using Microsoft.EntityFrameworkCore;

namespace RestApi.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options):
            base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=ProType12Felorez"
            );
        }

        public DbSet<User> Users { get; set; }
    }
}
