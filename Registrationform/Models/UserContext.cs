using Microsoft.EntityFrameworkCore;

namespace Registrationform.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) 
        {
            
        }
        public DbSet<Userdetails> UserDetails { get; set; }

        public DbSet<Country> Countries { get; set; }

        public  DbSet<SecQuestions> Squestion { get; set; }


        public  DbSet<State> States { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<ForgotPassword> ForgotPasswords { get; set; }


    }
}
