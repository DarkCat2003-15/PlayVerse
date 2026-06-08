using Microsoft.EntityFrameworkCore;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Interfaces.Repositories;

namespace SteamInfrastructure.Persistence.SqlServer.Repositories
{
    public class RoleRepository(SteamContext context) : IRoleRepository
    {
        public Task<List<Role>> GetActiveRoles()
        {
            return context.Roles
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public Task<Role?> GetById(Guid id)
        {
            return context.Roles
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public Task<Role?> GetByName(string name)
        {
            return context.Roles
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x => x.Name == name && x.IsActive);
        }

        public Task<List<Role>> GetByUserId(Guid userId)
        {
            return context.UserRoles
                .Include(x => x.Role)
                    .ThenInclude(x => x.RolePermissions)
                        .ThenInclude(x => x.Permission)
                .Where(x => x.UserId == userId && x.Role.IsActive)
                .Select(x => x.Role)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByUserId(Guid userId)
        {
            var permissions = await context.UserRoles
                .Include(x => x.Role)
                    .ThenInclude(x => x.RolePermissions)
                        .ThenInclude(x => x.Permission)
                .Where(x => x.UserId == userId && x.Role.IsActive)
                .SelectMany(x => x.Role.RolePermissions.Select(y => y.Permission))
                .Where(x => x.IsActive)
                .OrderBy(x => x.Code)
                .ToListAsync();

            return permissions
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToList();
        }

        public Task<List<Permission>> GetPermissions()
        {
            return context.Permissions
                .Where(x => x.IsActive)
                .OrderBy(x => x.Code)
                .ToListAsync();
        }

        public Task<bool> UserHasRole(Guid userId, string roleName)
        {
            return context.UserRoles.AnyAsync(x => x.UserId == userId && x.Role.Name == roleName && x.Role.IsActive);
        }

        public Task<bool> AssignmentExists(Guid userId, Guid roleId)
        {
            return context.UserRoles.AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
        }

        public async Task<UserRole> AssignRole(Guid userId, Guid roleId, Guid? assignedBy)
        {
            var entity = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedBy = assignedBy,
                AssignedAt = DateTime.UtcNow
            };

            await context.UserRoles.AddAsync(entity);
            return entity;
        }

        public async Task<bool> RemoveRole(Guid userId, Guid roleId)
        {
            var entity = await context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
            if (entity == null)
            {
                return false;
            }

            context.UserRoles.Remove(entity);
            return true;
        }
    }
}
