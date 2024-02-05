using Microsoft.EntityFrameworkCore;

namespace ExileTrack.Models;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public DbSet<ExileItem> GetItems { get; set; } = null!;
}