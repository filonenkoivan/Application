using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Co_Working.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Workspace> Workspace { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
