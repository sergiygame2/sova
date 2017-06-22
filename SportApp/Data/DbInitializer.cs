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
                    GymRate = 8,
                    GymLocation = "г. Киев ул. Маршала Якубовского 8",
                    Region = "Голосіївська",
                    MbrshipPrice = 395,
                    GymArea = 4,
                    FoundYear = 8,
                    Facilities = "тренер",
                    Url = "http://igym.com.ua/",
                    Description = "ТРЕНАЖЕРНЫЙ ЗАЛ, ФИТНЕС, ЙОГА, СROSSFIT И БОЙЦОВСКИЙ КЛУБ В ГОЛОСЕЕВСКОМ РАЙОНЕ КИЕВА",
                    GymImgUrl = "images/upload/iGYM.png",
                    Comments = null,
                    Latitude = "50.382568",
                    Longitude = "30.4522403",
   
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "MegaGYM",
                    GymRate = 8,
                    GymLocation = "г. Киев, Харьковское Шоссе, 19",
                    Region = "Позняки",
                    MbrshipPrice = 695,
                    GymArea = 4,
                    FoundYear = 7,
                    Facilities = "басейн, джакузі, масаж, солярій",
                    Url = "http://megagym.com/",
                    Description = "ТРЕНАЖЕРНЫЙ ЗАЛ, ФИТНЕС, ЙОГА, СROSSFIT НА ЛЕВОМ БЕРЕГУ ГОРОДА КИЕВ",
                    GymImgUrl = "images/upload/MegaGYM.jpg",
                    Comments = null,
                    Latitude = "50.431922",
                    Longitude = "30.6321253",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "DOG & Grand CrossFit",
                    GymRate = 10,
                    GymLocation = "г. Киев, прт. Степана Бандеры 6",
                    Region = "Петрівка",
                    MbrshipPrice = 695,
                    GymArea = 7,
                    FoundYear = 7,
                    Facilities = "тренер, сауна",
                    Url = "https://dogsportclub.com.ua/",
                    Description = "Мы – первый фитнес клуб Киева, который получил официальную CrossFit сертификацию. Высокие стандарты обслуживания, качества оборудования и подхода к тренировкам обязательны для получения сертификации.Титулованный тренерский состав и профессиональное оборудование сделают ваши занятия более эффективными и помогут выйти на новый уровень физической подготовки.",
                    GymImgUrl = "images/upload/DOG.jpg",
                    Comments = null,
                    Latitude = "50.486904",
                    Longitude = "30.4916939",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "KAWABANGA CROSS GYM",
                    GymRate = 7,
                    GymLocation = "г. Киев ул. Коновальца 44А",
                    Region = "Голосіївська",
                    MbrshipPrice = 577,
                    GymArea = 7,
                    FoundYear = 3,
                    Facilities = "басейн, тренер",
                    Url = "http://kawabanga.com.ua/",
                    Description = "",
                    GymImgUrl = "images/upload/kawabande.jpg",
                    Comments = null,
                    Latitude = "50.4253611",
                    Longitude = "30.5352724",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "CrossFit Voin",
                    GymRate = 8,
                    GymLocation = "г. Киев, ул. Обсерваторная 7",
                    Region = "Лісова",
                    MbrshipPrice = 389,
                    GymArea = 7,
                    FoundYear = 9,
                    Facilities = "full",
                    Url = "http://crossfitvoin.com/",
                    Description = "Это больше, чем просто фитнес зал. Наше сообщество основывается на желании помогать друг другу становится лучше, как снаружи так и внутри. Здесь ты сможешь улучшить свою физическую форму, выносливость, стать сильнее и здоровее. Типичный тренировочный день включает в себя разминку, изучение и отработку новых движений, WOD повышенной интенсивности, работу над ошибками и растяжку. Неважно какой твой род деятельности, здесь ты найдешь людей, которых объединяет одна цель - бросать вызов самому себе изо дня в день и становится лучше.",
                    GymImgUrl = "images/upload/voin.jpg",
                    Comments = null,
                    Latitude = "50.453269",
                    Longitude = "30.4987648",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "CrossFit Banda Подол",
                    GymRate = 9,
                    GymLocation = "г. Киев, Набережно Луговая, 4-А",
                    Region = "Поділ",
                    MbrshipPrice = 600,
                    GymArea = 7,
                    FoundYear = 6,
                    Facilities = "full",
                    Url = "http://www.crossfitbanda.com/",
                    Description = "Мы - Crossfit Banda, сообщество людей, увлеченных Crossfit.Первый и единственный в Украине сетевой Crossfit клуб,позволяющий тренироваться сразу в нескольких клубах сети по одной карте!",
                    GymImgUrl = "images/upload/CrossFit_Banda.jpg",
                    Comments = null,
                    Latitude = "50.4853548",
                    Longitude = "30.4458622",

                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "Crossfit Strong & Real Club",
                    GymRate = 9,
                    GymLocation = "г. Киев, ул. Фрунзе 46 ",
                    Region = "Поділ",
                    MbrshipPrice = 500,
                    GymArea = 7,
                    FoundYear = 12,
                    Facilities = "full",
                    Url = "http://crossfitsr.com.ua/",
                    Description = "Мы создали первый в Киеве специализированный Кроссфит зал на Подоле! Мы хотим дать вам возможность окунуться в этот волшебный Мир оздоровления, преодоления себя, выхода за кем-то очерченные для ВАС рамки!",
                    GymImgUrl = "images/upload/CROSS_fit_real_club.jpg",
                    Comments = null,
                    Latitude = "50.4729291",
                    Longitude = "30.4959457",

                });


                _gymsRepo.Add(new Gym()
                {
                    GymName = "Стальная Крыса",
                    GymRate = 5,
                    GymLocation = "г. Киев, ул. Нагорная 22 ",
                    Region = "Тараса Шевченка",
                    MbrshipPrice = 100,
                    GymArea = 7,
                    FoundYear = 3,
                    Facilities = "full",
                    Url = "http://extremefire.com.ua/",
                    Description = "Специализированный кроссфит-бокс Стальная Крыса в Шевченковском районе Киева. Мы стремимся следовать лучшим стандартам кроссфита, адаптируя тренировки для людей с разным уровнем подготовки. ",
                    GymImgUrl = "images/upload/krisa.jpg",
                    Comments = null,
                    Latitude = "50.4719285",
                    Longitude = "30.4815947",
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "Фитнес Цех",
                    GymRate = 6,
                    GymLocation = "г. Киев, ул. Чавдар 13",
                    Region = "Позняки",
                    MbrshipPrice = 155,
                    GymArea = 7,
                    FoundYear = 14,
                    Facilities = "full",
                    Url = "http://www.fitpro.com.ua/",
                    Description = "Студия персональных тренировок Фитнес Цеx на Осокорках. В студии проводятся тренировки по похудению,наращиванию мышц, функциональный тренинг(кроссфит), TRX, общие физические нагрузки.",
                    GymImgUrl = "images/upload/FIT_CEH.jpg",
                    Comments = null,
                    Latitude = "50.3909827",
                    Longitude = "30.6231987",
                });


                _gymsRepo.Add(new Gym()
                {
                    GymName = "КБ 2",
                    GymRate = 4,
                    GymLocation = "г. Киев, ул. Сосюры, 5",
                    Region = "Дарниця",
                    MbrshipPrice = 250,
                    GymArea = 7,
                    FoundYear = 9,
                    Facilities = "full",
                    Url = "http://kb2.com.ua/",
                    Description = "Кроссфит – тренировки построены на высокой интенсивности, включают элементы тяжелой и легкой атлетики, плиометрики и гимнастики и других видах спорта. Кроссфит сделает Вас сильным и выносливым, тело будет мощнее и функциональнее.",
                    GymImgUrl = "images/upload/KB_2.jpg",
                    Comments = null,
                    Latitude = "50.443585",
                    Longitude = "30.6288833",
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "Лавр",
                    GymRate = 2,
                    GymLocation = "г. Киев, ул. Б.Гмыры, 9-В",
                    Region = "Позняки",
                    MbrshipPrice = 436,
                    GymArea = 7,
                    FoundYear = 2,
                    Facilities = "full",
                    Url = "https://lavr-sportclub.kiev.ua/",
                    Description = "Цель нашего Спортивного клуба «Лавр» - в максимально быстрые сроки, качественно и эффективно достичь поставленного результата нашего посетителя, а помогут Вам в этом наши лучшие тренеры-профессионалы.",
                    GymImgUrl = "images/upload/LAVR.jpg",
                    Comments = null,
                    Latitude = "50.3938453",
                    Longitude = "30.6284022",
                });




                _gymsRepo.Add(new Gym()
                {
                    GymName = "Greka MMA",
                    GymRate = 8,
                    GymLocation = "г. Киев, ул. Жашковская, 20",
                    Region = "Поділ",
                    MbrshipPrice = 350,
                    GymArea = 7,
                    FoundYear = 6,
                    Facilities = "full",
                    Url = "www.kievsambo.com.ua/",
                    Description = "В просторном зале, площадью 150 кв.м. и оборудованном  всем необходимым снаряжением, занятия проходят  комфортно и максимально эффективно. Удобное время для тренировок.",
                    GymImgUrl = "images/upload/Greka.jpg",
                    Comments = null,
                    Latitude = "50.499611",
                    Longitude = "30.4445593",
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "Fitshine",
                    GymRate = 7,
                    GymLocation = "г. Киев, ул. Воскресенская, 14а",
                    Region = "Дарниця",
                    MbrshipPrice = 412,
                    GymArea = 7,
                    FoundYear = 7,
                    Facilities = "full",
                    Url = "https://fitshine.kiev.ua",
                    Description = "В фитнес-клубе «Fitshine» можно посетить просторный тренажерный зал, который оборудован новыми американскими тренажерами, кардио линией и свободными весами.",
                    GymImgUrl = "images/upload/Fitshine.jpg",
                    Comments = null,
                    Latitude = "50.4684435",
                    Longitude = "30.5996419",
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "MyFit",
                    GymRate = 3,
                    GymLocation = "г. Киев, ул. Ушинского, 28",
                    Region = "Святошино",
                    MbrshipPrice = 259,
                    GymArea = 7,
                    FoundYear = 9,
                    Facilities = "full",
                    Url = "https://myfit.ua",
                    Description = "Вместительный фитнес клуб «MyFit» рад приветствовать всех, кто желает стать частью движения за здоровый образ жизни и стремится сделать свое тело сильнее и стройнее.",
                    GymImgUrl = "images/upload/myfit.jpg",
                    Comments = null,
                    Latitude = "50.4293155",
                    Longitude = "30.4433288",
                });

                _gymsRepo.Add(new Gym()
                {
                    GymName = "БиоРитм",
                    GymRate = 6,
                    GymLocation = "г. Киев, Русановский бульвар, 12",
                    Region = "Русанівка",
                    MbrshipPrice = 459,
                    GymArea = 7,
                    FoundYear = 13,
                    Facilities = "full",
                    Url = "https://bio-ritm.com.ua",
                    Description = "Для каждого человека на персональных тренировках мы всегда разрабатываем индивидуальную диету и методику занятий. Этот комплекс услуг дает возможность привести тело клиента в отличную форму в наиболее короткий срок.",
                    GymImgUrl = "images/upload/bioritm.jpg",
                    Comments = null,
                    Latitude = "50.4381515",
                    Longitude = "30.5938638",
                });


            }
        }
    }
}
