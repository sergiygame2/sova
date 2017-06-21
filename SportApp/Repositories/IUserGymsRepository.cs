using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models;

namespace SportApp.Repositories
{
    public interface IUserGymsRepository : IModelRepository<UsersGyms>
    {
        List<Gym> GetGymsByUserId(string userID);
        UsersGyms GetByUserIdAndGymId(string userId, int gymId);

    }

    public class UserGymsRepository : GenericModelRepository<UsersGyms>, IUserGymsRepository
    {
        private readonly ApplicationDbContext _context;

        public UserGymsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Gym> GetGymsByUserId(string userId)
        {
            return _context.UsersGyms.Where(ug => ug.ApplicationUserId == userId).AsNoTracking()
                .Select(ug => ug.Gym).ToList();
        }

        public UsersGyms GetByUserIdAndGymId(string userId, int gymId) => 
            _context.UsersGyms.AsNoTracking().Where(ug => ug.ApplicationUserId == userId && ug.GymId == gymId).FirstOrDefault();
        
    }
}
