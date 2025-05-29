using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.Repository
{
    public class BookingRepository(AppDbContext db) : IBookingRepository
    {
        public async Task AddAsync(Booking booking)
        {
            await db.Bookings.AddAsync(booking);

            //var currentRooms = await db.Workspace.FirstOrDefaultAsync(x => x.WorkSpaceType == booking.WorkSpaceType);
            //if (currentRooms == null)
            //{
            //    return;
            //}

            //var itemForDelete = currentRooms.AvailabilityRooms.FirstOrDefault(x => x.Capacity == booking.RoomCapacity);
            //var newCurrrentRoom = currentRooms?.AvailabilityRooms.Remove(itemForDelete);

            await db.SaveChangesAsync();
        }

        public async Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime)
        {
            return await db.Bookings.AnyAsync(x => x.StartDateTime < endDateTime && x.EndDateTime > startDateTime);
        }
    }
}
