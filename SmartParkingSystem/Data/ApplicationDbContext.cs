using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Models;

namespace SmartParkingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingSession> ParkingSessions { get; set; }
    }
}