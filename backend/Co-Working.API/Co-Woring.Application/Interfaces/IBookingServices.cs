using Co_Woring.Application.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Interfaces
{
    public interface IBookingServices
    {
        public Task CreateAsync(BookingRequest request);
    }
}
