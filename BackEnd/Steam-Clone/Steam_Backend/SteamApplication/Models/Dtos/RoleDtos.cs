namespace SteamApplication.Models.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Specificity { get; set; } = string.Empty;
    }

    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionDto> Permissions { get; set; } = [];
    }

    public class UserRoleDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
        public Guid? AssignedBy { get; set; }
    }
}
