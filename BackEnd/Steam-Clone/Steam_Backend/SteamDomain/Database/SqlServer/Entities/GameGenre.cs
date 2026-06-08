using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class GameGenre
{
    public int GameGenreId { get; set; }

    public Guid? GameId { get; set; }

    public int? GenreId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual Genre? Genre { get; set; }
}
