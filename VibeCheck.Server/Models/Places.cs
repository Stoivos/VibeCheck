using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCheck.Server.Models;

[Table("tbl_places")]
public class Places
{
    [Key]
    [Column("pl_id")]
    public int Id { get; set; }

    [Column("pl_name")]
    public string Name { get; set; } = string.Empty;

    [Column("pl_latitude")]
    public double Latitude { get; set; }

    [Column("pl_longitude")]
    public double Longitude { get; set; }

    [Column("pl_radius")]
    public double RadiusMeters { get; set; }

    [Column("pl_imageurl")]
    public string? ImageUrl { get; set; }

    public List<Presence> Presences { get; set; } = [];
}