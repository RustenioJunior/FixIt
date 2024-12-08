using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class PartsContext : DbContext
{
   public PartsContext(DbContextOptions<PartsContext> options) : base(options)
    {
    }

    public DbSet<Parts> Partss { get; set; }
}

