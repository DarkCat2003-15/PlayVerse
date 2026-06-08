using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Session
{
    public int SessionId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GameId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
