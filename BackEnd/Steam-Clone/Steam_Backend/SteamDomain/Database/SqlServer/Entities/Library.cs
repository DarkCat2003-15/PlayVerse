using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Library
{
    public int LibraryId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GameId { get; set; }

    public decimal? PurchasePrice { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
