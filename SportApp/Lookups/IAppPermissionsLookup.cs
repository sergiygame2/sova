using System.Collections.Generic;

namespace SportApp.Lookups
{
    public interface IAppPermissionsLookup : IGymPermissionsLookup, ICommentPermissionsLookup, IUserPermissionsLookup, IRolePermissionLookup
    {
        string PermissionPattern { get; }
        IEnumerable<string> All();
    }

    public interface IGymPermissionsLookup
    {
        string ViewGyms { get; }
        string CreateGyms { get; }
        string UpdateGyms { get; }
        string RemoveGyms { get; }
    }

    public interface ICommentPermissionsLookup
    {
        string ViewComments { get; }
        string CreateComments { get; }
        string UpdateComments { get; }
        string RemoveComments { get; }
    }

    public interface IUserPermissionsLookup
    {
        string ViewUsers { get; }
        string CreateUsers { get; }
        string UpdateUsers { get; }
        string RemoveUsers { get; }
    }

    public interface IRolePermissionLookup
    {
        string ViewRoles { get; }
        string CreateRoles { get; }
        string UpdateRoles { get; }
        string RemoveRoles { get; }
    }
}
