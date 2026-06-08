using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class ReviewAnswer
{
    public int ReviewAnswersId { get; set; }

    public int ReviewId { get; set; }

    public Guid UserId { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Review Review { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
