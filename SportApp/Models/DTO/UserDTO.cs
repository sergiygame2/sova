using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportApp.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public DateTime BirthDay { get; set; }

        public int Height { get; set; }

        public double Weight { get; set; }

        public IDictionary<string, bool> Roles { get; set; }
    }
}
