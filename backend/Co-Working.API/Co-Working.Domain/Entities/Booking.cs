using Co_Working.Domain.Enums;
using Co_Working.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public WorkSpaceType WorkSpaceType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int SessionId { get; set; }
        public int RoomCapacity { get; set; }
        public int DeskNumber { get; set; }



        public int? DeskId { get; set; }
        public Desk? Desk { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
