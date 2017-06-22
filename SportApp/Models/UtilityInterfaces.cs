using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportApp.Models
{
    public interface IIdentifiable
    {
        int Id { get; set; }
    }
}
