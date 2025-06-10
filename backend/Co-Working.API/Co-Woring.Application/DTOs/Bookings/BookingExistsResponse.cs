using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.DTOs.Bookings
{
    public class BookingExistsResponse
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Type { get; set; }
        public bool Exists { get; set; }

        public int Capacity { get; set; }
    }
}
