using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SportApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public DateTime BirthDay { get; set; }

        public int Height { get; set; }

        public double Weight { get; set; }       
    }
}
