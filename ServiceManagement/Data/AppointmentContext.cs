using Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppointmentContext : DbContext
{
   public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; }
}

