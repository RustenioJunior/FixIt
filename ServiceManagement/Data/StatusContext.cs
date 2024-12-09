using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class StatusContext : DbContext
{
   public StatusContext(DbContextOptions<StatusContext> options) : base(options)
    {
    }

    public DbSet<Status> Statuss { get; set; }
}

