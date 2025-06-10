using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Workspace.Any())
            {
                context.AddRange(
                    new Workspace
                    {
                        Name = "Open space",
                        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                        WorkSpaceType = WorkSpaceType.OpenSpace,
                        PhotoList = new List<string> {
                        "images/workspaces/Openspace.jpg",
                        "images/workspaces/Openspace-2.jpg",
                        "images/workspaces/Openspace-3.jpg",
                        "images/workspaces/Openspace-4.jpg"
                    },
                        Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
                        AvailabilityDesks = Enumerable.Range(1, 24)
                        .Select(_ => new Desk())
                        .ToList()
                    },
                    new Workspace
                    {
                        Name = "Private rooms",
                        Description = "Ideal for focused work, video calls, or small team huddles. These fully enclosed rooms offer privacy and come in a variety of sizes to fit your needs.",
                        WorkSpaceType = WorkSpaceType.PrivateRoom,
                        PhotoList = new List<string> {
                            "images/workspaces/Privateroom.jpg",
                            "images/workspaces/Privateroom-2.jpg",
                            "images/workspaces/Privateroom.jpg",
                            "images/workspaces/Openspace-4.jpg"
                        },
                        Amenities = new List<string> { "wifi", "conditioner", "music" },
                        Capacity = new List<int> { 1, 2, 5, 10 },
                        AvailabilityRooms = new List<Room> {
                            new Room {Capacity = 1, Quantity = 7},
                            new Room {Capacity = 2, Quantity = 4},
                            new Room {Capacity = 5, Quantity = 3},
                            new Room {Capacity = 10, Quantity = 1},
                        },

                    },
                    new Workspace
                    {
                        Name = "Meeting rooms",
                        Description = "Designed for productive meetings, workshops, or client presentations. Equipped with screens, whiteboards, and comfortable seating to keep your sessions running smoothly.",
                        WorkSpaceType = WorkSpaceType.MeetingRoom,
                        PhotoList = new List<string> {
                            "images/workspaces/Meetingroom.jpg",
                            "images/workspaces/Meetingroom-2.jpg",
                            "images/workspaces/Meetingroom.jpg",
                            "images/workspaces/Meetingroom-4.jpg"
                        },
                        Amenities = new List<string> { "wifi", "conditioner", "music" },
                        Capacity = new List<int> { 10, 20 },
                        AvailabilityRooms = new List<Room> {
                            new Room { Capacity = 10, Quantity= 4},
                            new Room { Capacity = 20, Quantity = 1},
                        },
                    });

                context.SaveChanges();
            }

            if (!context.Bookings.Any())
            {
                context.AddRange(new Booking
                {
                    Name = "Ivan",
                    Email = "email@gmail.com",
                    StartDateTime = DateTime.UtcNow.AddDays(2),
                    EndDateTime = DateTime.UtcNow.AddDays(4),
                    RoomCapacity = 10,
                    WorkSpaceType = WorkSpaceType.MeetingRoom,
                    Id = 1,
                    RoomId = 2
                }, new Booking
                {
                    Name = "Ivan",
                    Email = "email@gmail.com",
                    StartDateTime = DateTime.UtcNow.AddDays(18),
                    EndDateTime = DateTime.UtcNow.AddDays(20),
                    DeskNumber = 6,
                    WorkSpaceType = WorkSpaceType.MeetingRoom,
                    Id = 2,
                    DeskId = 6
                });
                context.SaveChanges();
            }
        }
    }
}


