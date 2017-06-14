using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models;

namespace SportApp.Repositories
{
    public interface IGymRepository : IModelRepository<Gym>
    {
        List<Gym> Search(string region = null, string street = null);
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

        public List<Gym> Search(string region = null, string street = null)
        {
            var gyms = _context.Gym.AsNoTracking();
            if(!string.IsNullOrEmpty(region))
                gyms = gyms.Where(gym => gym.GymLocation.Contains(region));
            if (!string.IsNullOrEmpty(street))
                gyms = gyms.Where(gym => gym.GymLocation.Contains(street));
            return gyms.ToList();
        }
    }
}
