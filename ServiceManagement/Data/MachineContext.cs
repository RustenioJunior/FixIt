using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class MachineContext : DbContext
{
   public MachineContext(DbContextOptions<MachineContext> options) : base(options)
    {
    }

    public DbSet<Machine> Machines { get; set; }
}

