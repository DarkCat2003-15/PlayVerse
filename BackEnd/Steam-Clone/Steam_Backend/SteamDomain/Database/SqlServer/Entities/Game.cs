using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Game
{
    public Guid GameId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public decimal? Price { get; set; }

    public Guid? DeveloperId { get; set; }

    public Guid? PublisherId { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();

    public virtual Developer? Developer { get; set; }

    public virtual ICollection<Dlc> Dlcs { get; set; } = new List<Dlc>();

    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public virtual ICollection<Library> Libraries { get; set; } = new List<Library>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
