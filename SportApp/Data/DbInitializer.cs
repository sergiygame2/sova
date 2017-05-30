using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportApp.Lookups;
using SportApp.Models;

namespace SportApp.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAppPermissionsLookup _permissions;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IAppPermissionsLookup permissions)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._permissions = permissions;
        }

        public void SeedData()
        {
            _context.Database.Migrate();

            AddUserWithRoleAdmin();
        }

        private void AddUserWithRoleAdmin()
        {
            var email = "admin@example.com";
            if (!_context.Users.Any(x => x.Email == email))
            {
                if (_userManager.FindByEmailAsync(email).GetAwaiter().GetResult() == null)
                {
                    ApplicationUser appUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                    _userManager.CreateAsync(appUser, "Password_1").GetAwaiter().GetResult();
                }
            }

            ApplicationUser user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            var roleName = "Admin";
            if (_roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult() == null)
            {
                _roleManager.CreateAsync(new IdentityRole() { Name = roleName }).GetAwaiter().GetResult();
            }

            var role = _roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();

            foreach (var permission in _permissions.All())
                if (_context.RoleClaims.Count(rc => rc.ClaimValue == permission) == 0)
                    _roleManager.AddClaimAsync(role, new Claim("permission", permission)).GetAwaiter().GetResult();
            
            if (_userManager.IsInRoleAsync(user, roleName).GetAwaiter().GetResult() == false)
                _userManager.AddToRoleAsync(user, roleName).GetAwaiter().GetResult();
        }
    }
}
