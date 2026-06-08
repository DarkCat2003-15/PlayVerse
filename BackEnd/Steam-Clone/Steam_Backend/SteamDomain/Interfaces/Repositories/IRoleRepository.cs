using SteamDomain.Database.SqlServer.Entities;

namespace SteamDomain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetActiveRoles();
        Task<Role?> GetById(Guid id);
        Task<Role?> GetByName(string name);
        Task<List<Role>> GetByUserId(Guid userId);
        Task<List<Permission>> GetPermissionsByUserId(Guid userId);
        Task<List<Permission>> GetPermissions();
        Task<bool> UserHasRole(Guid userId, string roleName);
        Task<bool> AssignmentExists(Guid userId, Guid roleId);
        Task<UserRole> AssignRole(Guid userId, Guid roleId, Guid? assignedBy);
        Task<bool> RemoveRole(Guid userId, Guid roleId);
    }
}
