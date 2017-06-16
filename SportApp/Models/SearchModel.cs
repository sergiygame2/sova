using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportApp.Models
{
    public class SearchModel
    {
        public string Region { get; set; }

        public string Street { get; set; }

        public int StartPrice { get; set; }

        public int EndPrice { get; set; }
        
        public string Facilities { get; set; } 
    }
}
