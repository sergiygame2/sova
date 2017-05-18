using Microsoft.EntityFrameworkCore;

namespace SportApp.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void SeedData()
        {
            _context.Database.Migrate();
        }
    }

}
