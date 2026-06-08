using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Status
{
    public int StatusId { get; set; }

    public string Code { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
