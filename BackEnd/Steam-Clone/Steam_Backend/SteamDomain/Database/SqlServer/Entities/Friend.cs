using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Friend
{
    public Guid UserId { get; set; }

    public Guid FriendId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual User FriendNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
