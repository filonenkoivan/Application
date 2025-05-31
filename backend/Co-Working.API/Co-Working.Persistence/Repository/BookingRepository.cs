using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
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

            var currentRooms = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == booking.WorkSpaceType);
            if (currentRooms == null)
            {
                return;
            }

            var itemForDelete = currentRooms.AvailabilityRooms.FirstOrDefault(x => x.Capacity == booking.RoomCapacity);
            var newCurrrentRoom = currentRooms?.AvailabilityRooms.Remove(itemForDelete);

            await db.SaveChangesAsync();
        }

        public async Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime)
        {
            return await db.Bookings.AnyAsync(x => x.StartDateTime < endDateTime && x.EndDateTime > startDateTime);
        }

        public async Task<List<WorkspaceResponse>> GetWorkspacesAsync()
        {
            return await db.Workspace
                .Select(x => new WorkspaceResponse
                {
                    Name = x.Name,
                    Description = x.Description,
                    WorkSpaceType = x.WorkSpaceType,
                    AvailabilityRooms = x.AvailabilityRooms
                    .Select(y => new RoomDTO
                    {
                        Capacity = y.Capacity,
                        Id = y.Id
                    }).ToList(),
                    Amenities = x.Amenities,
                    Capacity = x.Capacity,
                    DescCount = x.DescCount,
                    PhotoList = x.PhotoList,
                }).ToListAsync();
        }

        public async Task<List<BookingResponse>> GetBookingsAsync()
        {
            return await db.Bookings.Select(x => new BookingResponse
            {
                Name = x.Name,
                SessionId = x.SessionId,
                Id = x.Id,
                Email = x.Email,
                StartDate = x.StartDateTime.Date,
                StartTime = x.StartDateTime.TimeOfDay,
                EndDate = x.EndDateTime.Date,
                EndTime = x.EndDateTime.TimeOfDay,
                WorkSpaceType = x.WorkSpaceType,
                RoomCapacity = x.RoomCapacity
            }).ToListAsync();
        }

        public async Task DeleteBooking(int id)
        {
            var itemForDelete = await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            db.Bookings.Remove(itemForDelete);

            var restoreRoom = await db.Workspace.FirstOrDefaultAsync(x => x.WorkSpaceType == itemForDelete.WorkSpaceType);
            restoreRoom.AvailabilityRooms.Add(new Room { Capacity = itemForDelete.RoomCapacity });

            await db.SaveChangesAsync();
        }

        public async Task UpdateBooking(int id, Booking booking)
        {
            db.Bookings.Update(booking);
            db.SaveChangesAsync();
        }
    }
}
