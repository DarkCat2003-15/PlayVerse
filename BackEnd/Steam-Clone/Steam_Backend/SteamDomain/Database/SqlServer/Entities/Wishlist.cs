using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Wishlist
{
    public int WishlistId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GameId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
