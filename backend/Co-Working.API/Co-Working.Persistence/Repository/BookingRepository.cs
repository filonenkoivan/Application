using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
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
        public async Task<Booking> AddAsync(Booking booking)
        {
            await db.Bookings.AddAsync(booking);

            var currentRooms = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == booking.WorkSpaceType);

            if (currentRooms == null)
            {
                return null;
            }

            var itemForDelete = currentRooms.AvailabilityRooms.FirstOrDefault(x => x.Capacity == booking.RoomCapacity);
            itemForDelete.Quantity--;

            await db.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime, DateTime startDate, int roomCapacity, WorkSpaceType workSpaceType)
        {
            var result = await db.Bookings.AnyAsync(x =>
            x.WorkSpaceType == workSpaceType &&
            x.RoomCapacity == roomCapacity &&
            x.StartDateTime.Date == startDate.Date &&
            x.StartDateTime < endDateTime &&
            x.EndDateTime > startDateTime);

            return result;
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
                        Id = y.Id,
                        Quantity = y.Quantity
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

        public async Task<bool> DeleteBooking(int id)
        {
            var itemForDelete = await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            db.Bookings.Remove(itemForDelete);

            if (itemForDelete == null)
            {
                return false;
            }

            var restoreRoom = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == itemForDelete.WorkSpaceType);
            restoreRoom.AvailabilityRooms.FirstOrDefault(x => x.Capacity == itemForDelete.RoomCapacity).Quantity++;

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<Room> GetRoomsByWorkspaceAndCapacity(WorkSpaceType type, int capacity)
        {
            var workSpace = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == type);
            return workSpace.AvailabilityRooms.FirstOrDefault(x => x.Capacity == capacity);
        }

        public async Task<BookingResponse> GetBookingAsync(int id)
        {
            return await db.Bookings.Select(x => new BookingResponse
            {
                Email = x.Email,
                RoomCapacity = x.RoomCapacity,
                StartDate = x.StartDateTime.Date,
                EndDate = x.EndDateTime,
                EndTime = x.EndDateTime.TimeOfDay,
                StartTime = x.StartDateTime.TimeOfDay,
                Id = x.Id,
                Name = x.Name,
                WorkSpaceType = x.WorkSpaceType

            }).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Booking> GetBookingEntityAsync(int id)
        {
            return await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type)
        {
            var workSpace = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == type);

            return workSpace.AvailabilityRooms.Select(x => new RoomDTO { Capacity = x.Capacity, Quantity = x.Quantity }).OrderBy(x => x.Capacity).ToList();
        }

        public async Task UpdateAsync(Booking booking)
        {
            db.Bookings.Update(booking);
            await db.SaveChangesAsync();
        }

        public async Task OpenSpaceDescsChange(WorkSpaceType workspace, bool increment)
        {
            if (increment)
            {
                db.Workspace.FirstOrDefault(x => x.WorkSpaceType == workspace).DescCount++;
            }
            else
            {
                db.Workspace.FirstOrDefault(x => x.WorkSpaceType == workspace).DescCount--;
            }
            await db.SaveChangesAsync();
        }
    }
}
