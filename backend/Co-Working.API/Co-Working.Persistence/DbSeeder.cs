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
            if (!context.Coworkings.Any())
            {
                context.AddRange(
                    new Coworking
                    {
                        Name = "WorkClub Pechersk",
                        Description = "Modern coworking in the heart of Pechersk with quiet rooms and coffee on tap.",
                        PhotoUrl = "images/coworking/coworking-1.jpg",
                        Address = "123 Yaroslaviv Val St, Kyiv",
                    }, new Coworking
                    {
                        Name = "UrbanSpace Podil",
                        Description = "A creative riverside hub ideal for freelancers and small startups.",
                        PhotoUrl = "images/coworking/coworking-2.jpg",
                        Address = "78 Naberezhno-Khreshchatytska St, Kyiv",
                    }, new Coworking
                    {
                        Name = "Creative Hub Lvivska",
                        Description = "A compact, design-focused space with open desks and strong community vibes.",
                        PhotoUrl = "images/coworking/coworking-3.jpg",
                        Address = "12 Lvivska Square, Kyiv",
                    }, new Coworking
                    {
                        Name = "TechNest Olimpiiska",
                        Description = "A high-tech space near Olimpiiska metro, perfect for team sprints and solo focus.",
                        PhotoUrl = "images/coworking/coworking-4.jpg",
                        Address = "45 Velyka Vasylkivska St, Kyiv",
                    }, new Coworking
                    {
                        Name = "Hive Station Troieshchyna",
                        Description = "A quiet, affordable option in the city's northeast—great for remote workers.",
                        PhotoUrl = "images/coworking/coworking-5.jpg",
                        Address = "102 Zakrevskogo St, Kyiv",
                    });
                context.SaveChanges();
            }
            if (!context.Workspace.Any())
            {
                context.AddRange(
                    new Workspace
                    {
                        CoworkingId = 1,
                        Name = "Open space",
                        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                        WorkSpaceType = WorkSpaceType.OpenSpace,
                        PhotoList = new List<string> {
                        "images/workspaces/Openspace.jpg",
                        "images/workspaces/Openspace-2.jpg",
                        "images/workspaces/Openspace-3.jpg",
                        "images/workspaces/Openspace-4.jpg",
                    },
                        Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
                        AvailabilityDesks = Enumerable.Range(1, 35)
                        .Select(_ => new Desk())
                        .ToList()
                    },
                    new Workspace
                    {
                        CoworkingId = 1,
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
                        CoworkingId = 1,
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
                    },
                    new Workspace
                    {
                        CoworkingId = 2,
                        Name = "Open space",
                        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                        WorkSpaceType = WorkSpaceType.OpenSpace,
                        PhotoList = new List<string> {
                        "images/workspaces/Openspace.jpg",
                        "images/workspaces/Openspace-2.jpg",
                        "images/workspaces/Openspace-3.jpg",
                        "images/workspaces/Openspace-4.jpg",
                    },
                        Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
                        AvailabilityDesks = Enumerable.Range(1, 20)
                        .Select(_ => new Desk())
                        .ToList()
                    },
                    new Workspace
                    {
                        CoworkingId = 2,
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
                            new Room {Capacity = 1, Quantity = 1},
                            new Room {Capacity = 2, Quantity = 3},
                        },

                    },
                    new Workspace
                    {
                        CoworkingId = 2,
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
                            new Room { Capacity = 20, Quantity = 1},
                        },
                    },
                    new Workspace
                    {
                        CoworkingId = 3,
                        Name = "Open space",
                        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                        WorkSpaceType = WorkSpaceType.OpenSpace,
                        PhotoList = new List<string> {
                        "images/workspaces/Openspace.jpg",
                        "images/workspaces/Openspace-2.jpg",
                        "images/workspaces/Openspace-3.jpg",
                        "images/workspaces/Openspace-4.jpg",
                    },
                        Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
                        AvailabilityDesks = Enumerable.Range(1, 15)
                        .Select(_ => new Desk())
                        .ToList()
                    },
                    new Workspace
                    {
                        CoworkingId = 3,
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
                        AvailabilityRooms = new List<Room>(),
                    },
                    new Workspace
                    {
                        CoworkingId = 3,
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
                            new Room { Capacity = 10, Quantity= 1},
                        },
                    },
                    new Workspace
                    {
                        CoworkingId = 4,
                        Name = "Open space",
                        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                        WorkSpaceType = WorkSpaceType.OpenSpace,
                        PhotoList = new List<string> {
                        "images/workspaces/Openspace.jpg",
                        "images/workspaces/Openspace-2.jpg",
                        "images/workspaces/Openspace-3.jpg",
                        "images/workspaces/Openspace-4.jpg",
                    },
                        Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
                        AvailabilityDesks = Enumerable.Range(1, 40)
                        .Select(_ => new Desk())
                        .ToList()
                    },
                    new Workspace
                    {
                        CoworkingId = 4,
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
                            new Room { Capacity = 1, Quantity= 1},
                            new Room { Capacity = 3, Quantity= 2},
                        }
                    },
                    new Workspace
                    {
                        CoworkingId = 4,
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
                            new Room { Capacity = 10, Quantity= 1},
                            new Room { Capacity = 20, Quantity= 1},
                        },
                    },
                    new Workspace
                    {
                        CoworkingId = 5,
                        Name = "Open space",
                        Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                        WorkSpaceType = WorkSpaceType.OpenSpace,
                        PhotoList = new List<string> {
                        "images/workspaces/Openspace.jpg",
                        "images/workspaces/Openspace-2.jpg",
                        "images/workspaces/Openspace-3.jpg",
                        "images/workspaces/Openspace-4.jpg",
                    },
                        Amenities = new List<string> { "conditioner", "game", "wifi", "tea" },
                        AvailabilityDesks = Enumerable.Range(1, 25)
                        .Select(_ => new Desk())
                        .ToList()
                    },
                    new Workspace
                    {
                        CoworkingId = 5,
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
                            new Room { Capacity = 3, Quantity= 1},
                        }
                    },
                    new Workspace
                    {
                        CoworkingId = 5,
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
                            new Room { Capacity = 20, Quantity= 1},
                        },
                    }
                    );
                context.SaveChanges();
            }

        }
    }
}


