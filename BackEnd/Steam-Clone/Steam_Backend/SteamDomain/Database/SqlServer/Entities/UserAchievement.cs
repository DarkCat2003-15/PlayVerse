using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class UserAchievement
{
    public int UserAchievementId { get; set; }

    public Guid? UserId { get; set; }

    public int? AchievementId { get; set; }

    public DateTime? UnlockedAt { get; set; }

    public virtual Achievement? Achievement { get; set; }

    public virtual User? User { get; set; }
}
