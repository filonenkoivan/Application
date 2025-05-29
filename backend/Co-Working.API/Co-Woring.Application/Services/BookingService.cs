using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Services
{
    public class BookingService(IBookingRepository repository) : IBookingServices
    {
        public async Task CreateAsync(BookingRequest request)
        {
            var startDateTime = request.StartDate + request.StartTime;
            var endDateTime = request.EndDate + request.EndTime;

            bool result = await repository.IsTimeOverlappingAsync(startDateTime, endDateTime);
            if (result)
            {
                return;
            }

            await repository.AddAsync(new Booking
            {
                Name = request.Name,
                Email = request.Email,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                SessionId = request.SessionId,
                WorkSpaceType = request.WorkSpaceType,
                RoomCapacity = request.RoomCapacity
            });
        }
    }
}
