using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class Review
{
    public int ReviewId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GameId { get; set; }

    public bool? IsRecommended { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Game? Game { get; set; }

    public virtual ICollection<ReviewAnswer> ReviewAnswers { get; set; } = new List<ReviewAnswer>();

    public virtual User? User { get; set; }
}
