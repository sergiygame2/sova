using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportApp.Lookups;
using SportApp.Models;
using SportApp.Repositories;

namespace SportApp.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAppPermissionsLookup _permissions;
        private readonly IGymRepository _gymsRepo;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IAppPermissionsLookup permissions,IGymRepository gymsRepo )
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._permissions = permissions;
            this._gymsRepo = gymsRepo;
        }

        public void SeedData()
        {
            _context.Database.Migrate();

            AddUserWithRoleAdmin();
            AddGyms();
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

        private void AddGyms()
        {
            if (!_gymsRepo.GetAll().Any())
            {
                _gymsRepo.Add(new Gym()
                {
                    GymName = "iGYM",
                    GymRate = 4,
                    GymLocation = "г. Киев ул. Маршала Якубовского 8",
                    Region = "Голосіївська",
                    MbrshipPrice = 195,
                    GymArea = 7,
                    FoundYear = 10,
                    Facilities = "тренер",
                    Url = "http://igym.com.ua/",
                    Description = "ТРЕНАЖЕРНЫЙ ЗАЛ, ФИТНЕС, ЙОГА, СROSSFIT И БОЙЦОВСКИЙ КЛУБ В ГОЛОСЕЕВСКОМ РАЙОНЕ КИЕВА",
                    GymImgUrl = "",
                    Comments = null,
                    Longitude = "50.382568",
                    Latitude = "30.4522403",
   
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "MegaGYM",
                    GymRate = 4,
                    GymLocation = "г. Киев, Харьковское Шоссе, 19",
                    Region = "Позняки",
                    MbrshipPrice = 195,
                    GymArea = 7,
                    FoundYear = 10,
                    Facilities = "басейн, джакузі, масаж, солярій",
                    Url = "http://megagym.com/",
                    Description = "ТРЕНАЖЕРНЫЙ ЗАЛ, ФИТНЕС, ЙОГА, СROSSFIT НА ЛЕВОМ БЕРЕГУ ГОРОДА КИЕВ",
                    GymImgUrl = "",
                    Comments = null,
                    Longitude = "50.431922",
                    Latitude = "30.6321253",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "DOG & Grand CrossFit",
                    GymRate = 4,
                    GymLocation = "г. Киев, прт. Степана Бандеры 6",
                    Region = "Петрівка",
                    MbrshipPrice = 195,
                    GymArea = 7,
                    FoundYear = 10,
                    Facilities = "тренер, сауна",
                    Url = "https://dogsportclub.com.ua/",
                    Description = "Мы – первый фитнес клуб Киева, который получил официальную CrossFit сертификацию. Высокие стандарты обслуживания, качества оборудования и подхода к тренировкам обязательны для получения сертификации.Титулованный тренерский состав и профессиональное оборудование сделают ваши занятия более эффективными и помогут выйти на новый уровень физической подготовки.",
                    GymImgUrl = "",
                    Comments = null,
                    Longitude = "50.486904",                 
                    Latitude = "30.4916939",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "KAWABANGA CROSS GYM",
                    GymRate = 4,
                    GymLocation = "г. Киев ул. Коновальца 44А",
                    Region = "Голосіївська",
                    MbrshipPrice = 1000,
                    GymArea = 7,
                    FoundYear = 10,
                    Facilities = "басейн, тренер",
                    Url = "http://kawabanga.com.ua/",
                    Description = "",
                    GymImgUrl = "",
                    Comments = null,
                    Longitude = "50.4253611",
                    Latitude = "30.5352724",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "CrossFit Voin",
                    GymRate = 4,
                    GymLocation = "г. Киев, ул. Обсерваторная 7",
                    Region = "Лісова",
                    MbrshipPrice = 1000,
                    GymArea = 7,
                    FoundYear = 10,
                    Facilities = "full",
                    Url = "http://crossfitvoin.com/",
                    Description = "Это больше, чем просто фитнес зал. Наше сообщество основывается на желании помогать друг другу становится лучше, как снаружи так и внутри. Здесь ты сможешь улучшить свою физическую форму, выносливость, стать сильнее и здоровее. Типичный тренировочный день включает в себя разминку, изучение и отработку новых движений, WOD повышенной интенсивности, работу над ошибками и растяжку. Неважно какой твой род деятельности, здесь ты найдешь людей, которых объединяет одна цель - бросать вызов самому себе изо дня в день и становится лучше.",
                    GymImgUrl = "",
                    Comments = null,
                    Longitude = "50.453269",
                    Latitude = "30.4987648",

                });

            }
        }
    }
}
