using Microsoft.EntityFrameworkCore;
using VibeCheck.Server.Models;

namespace VibeCheck.Server.Data;

public class VibeCheckDbContext : DbContext
{
    public VibeCheckDbContext(DbContextOptions<VibeCheckDbContext> options)
        : base(options)
    {
    }

    public DbSet<Places> Places => Set<Places>();
    public DbSet<Presence> Presences => Set<Presence>();
}