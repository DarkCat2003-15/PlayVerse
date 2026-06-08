using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Offer
{
    public int OfferId { get; set; }

    public Guid? GameId { get; set; }

    public int? Discount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Game? Game { get; set; }
}
