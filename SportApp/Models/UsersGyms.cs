using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportApp.Models
{
    public class UsersGyms
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int GymId { get; set; }
        public Gym Gym { get; set; }
    }
}
