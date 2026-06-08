using System;
using System.Collections.Generic;

namespace SteamDomain.Database.SqlServer.Entities;

public partial class EmailTemplate
{
    public int EmailTemplateId { get; set; }

    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
