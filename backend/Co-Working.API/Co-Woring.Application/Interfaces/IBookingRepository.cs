using Co_Woring.Application.DTOs.Booking;
using Co_Working.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Interfaces
{
    public interface IBookingRepository
    {
        public Task AddAsync(Booking booking);
        public Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime);
    }
}
