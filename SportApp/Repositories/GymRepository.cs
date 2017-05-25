using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models;

namespace SportApp.Repositories
{
    public class GymRepository : GenericModelRepository<Gym>
    {
        private readonly ApplicationDbContext _context;

        public GymRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override Gym Get(int id) => 
            _context.Set<Gym>().AsNoTracking().Where(gym => gym.Id == id).Include(gym => gym.Comments).FirstOrDefault();

        public override List<Gym> GetAll() => _context.Set<Gym>().AsNoTracking().Include(gym => gym.Comments).ToList();
    }
}
