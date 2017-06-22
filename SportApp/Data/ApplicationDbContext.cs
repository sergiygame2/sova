using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SportApp.Models;
using SportApp.Models.DTO;

namespace SportApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Gym> Gym { get; set; }
        public DbSet<UsersGyms> UsersGyms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        
        }

        public DbSet<SportApp.Models.DTO.UserDTO> UserDTO { get; set; }
    }
}
