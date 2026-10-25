using Microsoft.EntityFrameworkCore;

namespace World.Api.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Models.Country> Countries { get; set; }
        public DbSet<Models.States> States { get; set; }
    }
}
