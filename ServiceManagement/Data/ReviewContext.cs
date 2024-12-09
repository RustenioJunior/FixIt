using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ReviewContext : DbContext
{
   public ReviewContext(DbContextOptions<ReviewContext> options) : base(options)
    {
    }

    public DbSet<Review> Reviews { get; set; }
}

