using SteamApplication.Helpers;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Roles;
using SteamApplication.Models.Response;
using SteamDomain.Database.SqlServer;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Exceptions;
using SteamShared.Constants;

namespace SteamApplication.Servicios
{
    public class RoleService(IUnitOfWork uow) : IRoleService
    {
        public async Task<GenericResponse<List<RoleDto>>> GetRoles()
        {
            var roles = await uow.roleRepository.GetActiveRoles();
            var data = roles.Select(MapRole).ToList();
            return ResponseHelper.Create(data, count: data.Count);
        }

        public async Task<GenericResponse<List<PermissionDto>>> GetPermissions()
        {
            var permissions = await uow.roleRepository.GetPermissions();
            var data = permissions.Select(MapPermission).ToList();
            return ResponseHelper.Create(data, count: data.Count);
        }

        public async Task<GenericResponse<List<RoleDto>>> GetMyRoles(Guid userId)
        {
            var roles = await uow.roleRepository.GetByUserId(userId);
            var data = roles.Select(MapRole).ToList();
            return ResponseHelper.Create(data, count: data.Count);
        }

        public async Task<GenericResponse<UserRoleDto>> AssignRole(AssignRoleRequest model, Guid assignedBy)
        {
            var role = model.RoleId.HasValue
                ? await uow.roleRepository.GetById(model.RoleId.Value)
                : !string.IsNullOrWhiteSpace(model.RoleName)
                    ? await uow.roleRepository.GetByName(model.RoleName)
                    : null;

            if (role == null)
            {
                throw new NotFoundException(ResponseConstants.ROLE_NOT_EXISTS);
            }

            var userExists = await uow.userRepository.IfExists(model.UserId);
            if (!userExists)
            {
                throw new NotFoundException(ResponseConstants.USER_NOT_EXISTS);
            }

            if (await uow.roleRepository.AssignmentExists(model.UserId, role.Id))
            {
                throw new BadRequestException(ResponseConstants.ROLE_ALREADY_ASSIGNED);
            }

            var assignment = await uow.roleRepository.AssignRole(model.UserId, role.Id, assignedBy);
            await uow.SaveChangesAsync();

            return ResponseHelper.Create(new UserRoleDto
            {
                UserId = assignment.UserId,
                RoleId = assignment.RoleId,
                RoleName = role.Name,
                AssignedAt = assignment.AssignedAt,
                AssignedBy = assignment.AssignedBy
            });
        }

        public async Task<GenericResponse<bool>> RemoveRole(Guid userId, Guid roleId)
        {
            var removed = await uow.roleRepository.RemoveRole(userId, roleId);
            if (!removed)
            {
                throw new NotFoundException(ResponseConstants.ROLE_NOT_EXISTS);
            }

            await uow.SaveChangesAsync();
            return ResponseHelper.Create(true);
        }

        private static RoleDto MapRole(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                Permissions = role.RolePermissions
                    .Where(x => x.Permission != null && x.Permission.IsActive)
                    .Select(x => MapPermission(x.Permission))
                    .DistinctBy(x => x.Id)
                    .ToList()
            };
        }

        private static PermissionDto MapPermission(Permission permission)
        {
            return new PermissionDto
            {
                Id = permission.Id,
                Code = permission.Code,
                Module = permission.Module,
                Action = permission.Action,
                Name = permission.Name,
                Description = permission.Description,
                Specificity = permission.Specificity
            };
        }
    }
}
