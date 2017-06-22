using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportApp.Lookups
{
    public class AppPermissionsLookup : IAppPermissionsLookup
    {
        public string PermissionPattern => @"(\w+)\s+([\w-]+)";

        public string ViewGyms => "view gyms";
        public string CreateGyms => "create gyms";
        public string UpdateGyms => "update gyms";
        public string RemoveGyms => "remove gyms";

        public string ViewComments => "view comments";
        public string CreateComments => "create comments";
        public string UpdateComments => "update comments";
        public string RemoveComments => "remove comments";

        public string ViewUsers => "view users";
        public string CreateUsers => "create users";
        public string UpdateUsers => "update users";
        public string RemoveUsers => "remove users";

        public string ViewRoles => "view roles";
        public string CreateRoles => "create roles";
        public string UpdateRoles => "update roles";
        public string RemoveRoles => "remove roles";

        public IEnumerable<string> All() => new List<string>()
        {
                ViewUsers,
                ViewRoles,
                ViewGyms,
                ViewComments,

                CreateUsers,
                CreateRoles,
                CreateGyms,
                CreateComments,

                UpdateUsers,
                UpdateRoles,
                UpdateGyms,
                UpdateComments,

                RemoveUsers,
                RemoveRoles,
                RemoveGyms,
                RemoveComments
         };
    }
}
