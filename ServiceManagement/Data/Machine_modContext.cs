using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class Machine_modContext : DbContext
{
   public Machine_modContext(DbContextOptions<Machine_modContext> options) : base(options)
    {
    }

    public DbSet<Machine_mod> Machine_mods { get; set; }
}

