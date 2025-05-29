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

            //modelBuilder.Entity<Workspace>().HasData(
            //    new Workspace
            //    {
            //        Name = "Open space",
            //        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
            //        WorkSpaceType = WorkSpaceType.OpenSpace,
            //        DescCount = 24,
            //        Id = 1
            //        //PhotoList = new List<string> {
            //        //    "images\\workspaces\\Openspace.jpg",
            //        //    "images\\workspaces\\Openspace-2.jpg",
            //        //    "images\\workspaces\\Openspace-3.jpg",
            //        //    "images\\workspaces\\Openspace-4.jpg"
            //        //},
            //        //Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
            //    },
            //    new Workspace
            //    {
            //        Name = "Private rooms",
            //        Description = "Ideal for focused work, video calls, or small team huddles. These fully enclosed rooms offer privacy and come in a variety of sizes to fit your needs.",
            //        WorkSpaceType = WorkSpaceType.PrivateRoom,
            //        Id = 2
            //        //PhotoList = new List<string> {
            //        //    "images\\workspaces\\Privateroom.jpg",
            //        //    "images\\workspaces\\Privateroom-2.jpg",
            //        //    "images\\workspaces\\Privateroom.jpg",
            //        //    "images\\workspaces\\Openspace-4.jpg"
            //        //},
            //        //Amenities = new List<string> { "wifi", "conditioner", "music" },
            //        //Capacity = new List<int> { 1, 2, 5, 10 },
            //        //AvailabilityRooms = new List<Room> {
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 1 },
            //        //    new Room { Capacity = 2 },
            //        //    new Room { Capacity = 2 },
            //        //    new Room { Capacity = 2 },
            //        //    new Room { Capacity = 2 },
            //        //    new Room { Capacity = 5 },
            //        //    new Room { Capacity = 5 },
            //        //    new Room { Capacity = 5 },
            //        //},

            //    },
            //    new Workspace
            //    {
            //        Name = "Meeting rooms",
            //        Description = "Designed for productive meetings, workshops, or client presentations. Equipped with screens, whiteboards, and comfortable seating to keep your sessions running smoothly.",
            //        WorkSpaceType = WorkSpaceType.MeetingRoom,
            //        Id = 3
            //        //PhotoList = new List<string> {
            //        //    "images\\workspaces\\Meetingroom.jpg",
            //        //    "images\\workspaces\\Meetingroom-2.jpg",
            //        //    "images\\workspaces\\Meetingroom.jpg",
            //        //    "images\\workspaces\\Meetingroom-4.jpg"
            //        //},
            //        //Amenities = new List<string> { "wifi", "conditioner", "music" },
            //        //Capacity = new List<int> { 10, 20 },
            //        //AvailabilityRooms = new List<Room> {
            //        //    new Room { Capacity = 10 },
            //        //    new Room { Capacity = 10 },
            //        //    new Room { Capacity = 10 },
            //        //    new Room { Capacity = 10 },
            //        //    new Room { Capacity = 20 },
            //        //},
            //    }
            //    );
        }
    }
}
