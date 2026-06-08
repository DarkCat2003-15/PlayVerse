using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Achievement
{
    public int AchievementId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public Guid? GameId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
