using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Bookings;
using Co_Woring.Application.DTOs.Desks;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using Co_Working.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.Repository
{
    public class BookingRepository(AppDbContext db) : IBookingRepository
    {
        public async Task<Booking> AddAsync(Booking booking)
        {
            bool isOverlapping = await IsTimeOverlappingAsync(
                booking.StartDateTime,
                booking.EndDateTime,
                booking.RoomCapacity,
                booking.WorkSpaceType,
                booking.DeskNumber);

            if (isOverlapping)
            {
                return null;
            }

            await db.Bookings.AddAsync(booking);
            await db.SaveChangesAsync();

            return booking;
        }
        public async Task UpdateAsync(Booking booking)
        {
            db.Bookings.Update(booking);
            await db.SaveChangesAsync();
        }
        public async Task<bool> DeleteBooking(int id)
        {
            var booking = await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            if (booking == null)
                return false;

            db.Bookings.Remove(booking);
            await db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime, int roomCapacity, WorkSpaceType workSpaceType, int deskNumber = 0)
        {
            var dateOnlyStart = startDateTime.Date;
            var dateOnlyEnd = endDateTime.Date;

            for (var date = dateOnlyStart; date <= dateOnlyEnd; date = date.AddDays(1))
            {
                var dailyStart = date == dateOnlyStart ? startDateTime.TimeOfDay : TimeSpan.Zero;
                var dailyEnd = date == dateOnlyEnd ? endDateTime.TimeOfDay : new TimeSpan(23, 59, 59);

                DateTime currentDayStart = date.Add(dailyStart);
                DateTime currentDayEnd = date.Add(dailyEnd);

                bool isOverlapping;

                if (roomCapacity == 0)
                {
                    isOverlapping = await db.Bookings.AnyAsync(x =>
                        x.WorkSpaceType == workSpaceType &&
                        x.RoomCapacity == 0 &&
                        x.DeskNumber == deskNumber &&
                        x.StartDateTime.Date == date &&
                        x.StartDateTime.TimeOfDay < dailyEnd &&
                        x.EndDateTime.TimeOfDay > dailyStart);
                }
                else
                {
                    isOverlapping = await db.Bookings.AnyAsync(x =>
                        x.WorkSpaceType == workSpaceType &&
                        x.RoomCapacity == roomCapacity &&
                        x.StartDateTime.Date == date &&
                        x.StartDateTime.TimeOfDay < dailyEnd &&
                        x.EndDateTime.TimeOfDay > dailyStart);
                }

                if (isOverlapping)
                    return true;
            }

            return false;
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
                    AvailabilityDesks = x.AvailabilityDesks
                    .Select(y => new DeskDTO
                    {
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
            var bookingResponse = await db.Bookings.Select(x => new BookingResponse
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
                RoomCapacity = x.RoomCapacity,
                DeskNumber = x.DeskNumber,
            }).ToListAsync();

            return bookingResponse;
        }
        public async Task<IBookable> GetBookableByWorkspaceAndCapacity(WorkSpaceType type, int capacity, int deskNumber = 0)
        {
            if (capacity == 0)
            {
                var result = await db.Workspace.Include(x => x.AvailabilityDesks).FirstOrDefaultAsync(x => x.WorkSpaceType == type);
                return result.AvailabilityDesks.FirstOrDefault(x => x.Id == deskNumber);
            }
            var workSpace = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == type);
            return workSpace.AvailabilityRooms.FirstOrDefault(x => x.Capacity == capacity);
        }
        public async Task<BookingResponse> GetBookingAsync(int id)
        {
            return await db.Bookings.Select(x => new BookingResponse
            {
                Email = x.Email,
                RoomCapacity = x.RoomCapacity,
                DeskNumber = x.DeskNumber,
                StartDate = x.StartDateTime.Date,
                EndDate = x.EndDateTime,
                EndTime = x.EndDateTime.TimeOfDay,
                StartTime = x.StartDateTime.TimeOfDay,
                Id = x.Id,
                Name = x.Name,
                WorkSpaceType = x.WorkSpaceType,


            }).FirstOrDefaultAsync(x => x.Id == id);

        }
        public async Task<Booking> GetBookingEntityAsync(int id)
        {
            return await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);

        }
        public async Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type)
        {
            var workSpace = await db.Workspace.Include(x => x.AvailabilityRooms).FirstOrDefaultAsync(x => x.WorkSpaceType == type);

            return workSpace.AvailabilityRooms.Select(x => new RoomDTO { Capacity = x.Capacity, Quantity = x.Quantity, Id = x.Id }).OrderBy(x => x.Capacity).ToList();
        }
        public async Task<List<DeskDTO>> GetDesksByType(WorkSpaceType type)
        {
            var workSpace = await db.Workspace.Include(x => x.AvailabilityDesks).FirstOrDefaultAsync(x => x.WorkSpaceType == type);

            return workSpace.AvailabilityDesks.Select(x => new DeskDTO { Quantity = x.Quantity, Id = x.Id }).ToList();
        }
        public async Task DecreaseAvailabilityAsync(Booking booking)
        {
            var workspace = await db.Workspace
                .Include(w => w.AvailabilityDesks)
                .Include(w => w.AvailabilityRooms)
                .FirstOrDefaultAsync(w => w.WorkSpaceType == booking.WorkSpaceType);

            if (booking.RoomCapacity > 0)
            {
                var room = workspace?.AvailabilityRooms.FirstOrDefault(x => x.Capacity == booking.RoomCapacity);
                if (room != null && room.Quantity > 0) room.Quantity--;
            }

            await db.SaveChangesAsync();
        }
        public async Task<List<BookingAvailableResponse>> GetBookingsByType(WorkSpaceType type, int capacity)
        {
            var workspace = db.Bookings.Where(x => x.WorkSpaceType == type);
            if (capacity == 0)
            {
                return await workspace.Select(x => new BookingAvailableResponse
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,

                }).ToListAsync();
            }
            else
            {
                return await workspace.Where(x => x.RoomCapacity == capacity).Select(x => new BookingAvailableResponse
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,

                }).ToListAsync();
            }
        }
        public async Task<List<BookingAvailableResponse>> GetBookingsDesks(int deskId)
        {
            var workspace = db.Bookings.Where(x => x.WorkSpaceType == WorkSpaceType.OpenSpace);

            return await workspace.Where(x => x.DeskNumber == deskId).Select(x => new BookingAvailableResponse
            {
                StartDateTime = x.StartDateTime,
                EndDateTime = x.EndDateTime,

            }).ToListAsync();
        }

        public async Task<bool> ExistsBookingWithSessionIdAndWorkspaceTypeAsync(int sessionId, WorkSpaceType workSpaceType)
        {
            return await db.Bookings.AnyAsync(x => x.WorkSpaceType == workSpaceType && x.SessionId == sessionId);
        }

        public async Task<BookingExistsResponse> GetBookingByWorkspaceAndSessionIdAsync(WorkSpaceType type, int id)
        {
            var result = await db.Bookings.FirstOrDefaultAsync(x => x.SessionId == id && x.WorkSpaceType == type);
            var enCulture = new CultureInfo("en-US");

            if (result == null)
            {
                return new BookingExistsResponse
                {
                    Exists = false
                };
            }
            return new BookingExistsResponse
            {
                StartDate = result.StartDateTime.Date.ToString("MMMM d, yyyy", enCulture),
                EndDate = result.EndDateTime.Date.ToString("MMMM d, yyyy", enCulture),
                Type = result.RoomCapacity == 0 ? "Room" : "Desk",
                Exists = true,
                Capacity = result.RoomCapacity
            };
        }
    }
}
