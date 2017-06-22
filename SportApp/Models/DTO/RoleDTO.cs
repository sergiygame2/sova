using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportApp.Models.DTO
{
    public class RoleDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IDictionary<string, Dictionary<string, bool>> Permissions { get; set; }
    }
}
