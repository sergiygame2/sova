using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models;

namespace SportApp.Repositories
{
    public interface IGymRepository : IModelRepository<Gym>
    {
        IQueryable<Gym> Search(string region = null, string street = null, int startPrice = 0, int endPrice = 0, List<string> facilities = null);
        IQueryable<Gym> GetIQueryable();
    }
    
    public class GymRepository : GenericModelRepository<Gym>, IGymRepository
    {
        private readonly ApplicationDbContext _context;

        public GymRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override Gym Get(int id) => 
            _context.Set<Gym>().AsNoTracking().Where(gym => gym.Id == id).Include(gym => gym.Comments).FirstOrDefault();

        public override List<Gym> GetAll() => _context.Set<Gym>().AsNoTracking().Include(gym => gym.Comments).ToList();

        public IQueryable<Gym> GetIQueryable() => _context.Gym.AsNoTracking();

        public IQueryable<Gym> Search(string region = null, string street = null, int startPrice = 0, int endPrice = 0, List<string> facilities = null)
        {
            var gyms = _context.Gym.AsNoTracking();
            if(!string.IsNullOrEmpty(region))
                gyms = gyms.Where(gym => gym.Region.Contains(region));
            if(!string.IsNullOrEmpty(street))
                gyms = gyms.Where(gym => gym.GymLocation.Contains(street));
            if (startPrice == 0&&endPrice!=0)
                gyms = gyms.Where(gym=>gym.MbrshipPrice<=endPrice);
            if (startPrice != 0 && endPrice == 0)
                gyms = gyms.Where(gym => gym.MbrshipPrice >= startPrice);
            if (startPrice != 0 && endPrice != 0)
                gyms = gyms.Where(gym => gym.MbrshipPrice <= endPrice && gym.MbrshipPrice>=startPrice);
            if (facilities != null)
                facilities.ForEach(facility => gyms = gyms.Where((gym => gym.Facilities.Contains(facility))));
            return gyms;
        }
    }
}
