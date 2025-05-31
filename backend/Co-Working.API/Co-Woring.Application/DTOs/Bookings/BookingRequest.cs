using Co_Working.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.DTOs.Booking
{
    public class BookingRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public WorkSpaceType WorkSpaceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomCapacity { get; set; }
        public int SessionId { get; set; }
    }
}
