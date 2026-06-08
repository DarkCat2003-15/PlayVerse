using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Developer
{
    public Guid DeveloperId { get; set; }

    public string? DeveloperName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
