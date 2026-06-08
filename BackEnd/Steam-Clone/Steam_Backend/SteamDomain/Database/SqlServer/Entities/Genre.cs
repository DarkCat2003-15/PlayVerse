using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}
