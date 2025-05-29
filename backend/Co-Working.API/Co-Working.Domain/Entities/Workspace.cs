using Co_Working.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Domain.Entities
{
    public class Workspace
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public WorkSpaceType WorkSpaceType { get; set; }
        public List<string> Amenities { get; set; } = new();
        public List<string> PhotoList { get; set; } = new();
        //for openspace
        public int DescCount { get; set; }
        //for private and meeting
        public List<Room> AvailabilityRooms { get; set; } = new();
        public List<int>? Capacity { get; set; }

    }
}

