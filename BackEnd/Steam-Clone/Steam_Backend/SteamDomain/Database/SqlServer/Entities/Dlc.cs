using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Dlc
{
    public int Dlcid { get; set; }

    public string? Dlcname { get; set; }

    public decimal? Price { get; set; }

    public DateTime? AddedAt { get; set; }

    public Guid? GameId { get; set; }

    public virtual Game? Game { get; set; }
}
