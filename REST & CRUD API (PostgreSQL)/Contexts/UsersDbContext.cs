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

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserIIN)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserIIN>(ui => ui.UserId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;Database=Users;Port=5432;User Id=postgres;Password=FRON12NORF"
            );
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserIIN> UserIINs { get; set; }
    }
}
