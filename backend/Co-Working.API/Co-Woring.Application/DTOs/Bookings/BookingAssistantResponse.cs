using Co_Working.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.DTOs.Bookings
{
    public class BookingAssistantResponse
    {
        public WorkSpaceType WorkSpaceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomCapacity { get; set; }
        public int DeskNumber { get; set; }
        public string Location { get; set; }
    }
}
