using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class CompanyContext : DbContext
{
   public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
}

