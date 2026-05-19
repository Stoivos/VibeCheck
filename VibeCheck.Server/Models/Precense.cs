using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCheck.Server.Models;

[Table("tbl_presence")]
public class Presence
{
    [Key]
    [Column("pr_id")]
    public int Id { get; set; }

    [Column("pr_sessionId")]
    public string SessionId { get; set; } = string.Empty;

    [Column("pr_timestamp")]
    public DateTime LastSeen { get; set; }

    [ForeignKey(nameof(Place))]
    [Column("pr_place")]
    public int PlaceId { get; set; }

    public Places? Place { get; set; }
}