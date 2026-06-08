using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? StatusId { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Friend> FriendFriendNavigations { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    public virtual ICollection<Library> Libraries { get; set; } = new List<Library>();

    public virtual ICollection<ReviewAnswer> ReviewAnswers { get; set; } = new List<ReviewAnswer>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual Status? Status { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();

    public virtual ICollection<UserRole> UserRoleAssignedByNavigations { get; set; } = new List<UserRole>();

    public virtual ICollection<UserRole> UserRoleUsers { get; set; } = new List<UserRole>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
