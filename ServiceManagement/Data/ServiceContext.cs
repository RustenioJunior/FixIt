using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ServiceContext : DbContext
{
   public ServiceContext(DbContextOptions<ServiceContext> options) : base(options)
    {
    }

    public DbSet<Service> Services { get; set; }
}

