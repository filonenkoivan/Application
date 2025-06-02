using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Co_Woring.Application.Services
{
    public class BookingService(IBookingRepository repository) : IBookingServices
    {
        public async Task<(bool Succes, string Text)> CreateAsync(BookingRequest request)
        {
            var validation = await ValidateBookingRequest(request);
            if (!validation.Success)
                return (false, validation.Message);

            var booking = await repository.AddAsync(new Booking
            {
                Name = request.Name,
                Email = request.Email,
                StartDateTime = validation.Start,
                EndDateTime = validation.End,
                SessionId = request.SessionId,
                WorkSpaceType = request.WorkSpaceType,
                RoomCapacity = request.RoomCapacity,
                Room = validation.Room
            });

            return (true, $"Your room for {booking.RoomCapacity} people is booked from " +
                          $"{booking.StartDateTime:MMMM d, yyyy} to {booking.EndDateTime:MMMM d, yyyy}. " +
                          $"Confirmation sent to {booking.Email}");
        }
        public async Task<(bool Success, string Message)> UpdateAsync(int id, BookingRequest request)
        {
            var booking = await repository.GetBookingEntityAsync(id);
            if (booking == null)
                return (false, "Booking not found");

            var validation = await ValidateBookingRequest(request, excludeBookingId: id);
            if (!validation.Success)
                return (false, validation.Message);

            booking.Name = request.Name;
            booking.Email = request.Email;
            booking.StartDateTime = validation.Start;
            booking.EndDateTime = validation.End;
            booking.WorkSpaceType = request.WorkSpaceType;
            booking.RoomCapacity = request.RoomCapacity;


            await repository.UpdateAsync(booking);

            return (true, "Booking updated successfully");
        }
        public async Task<List<BookingResponse>> GetBookings()
        {
            return await repository.GetBookingsAsync();
        }

        public async Task<Response<string>> DeleteBooking(int id)
        {
            var result = await repository.DeleteBooking(id);

            if (result == true)
            {
                return new Response<string>(DTOs.Enums.ApiStatusCode.NotFound, "Booking not found");
            }
            else
            {
                return new Response<string>(DTOs.Enums.ApiStatusCode.Success, "Booking deleted");
            }
        }


        public async Task<BookingResponse> GetBookingAsync(int id)
        {
            return await repository.GetBookingAsync(id);
        }

        public async Task<List<WorkspaceResponse>> GetWorkspacesAsync()
        {
            return await repository.GetWorkspacesAsync();
        }

        public async Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type)
        {
            return await repository.GetRoomsByType(type);
        }

        private async Task<(bool Success, string Message, Room Room, DateTime Start, DateTime End)>
        ValidateBookingRequest(BookingRequest request, int? excludeBookingId = null)
        {
            var startDateTime = request.StartDate + request.StartTime;
            var endDateTime = request.EndDate + request.EndTime;

            if (request.WorkSpaceType == WorkSpaceType.OpenSpace)
            {
                var workspaces = await GetWorkspacesAsync();

                bool overlappingOpenSpace = await repository.IsTimeOverlappingAsync(
                    startDateTime, endDateTime, request.StartDate,
                    request.RoomCapacity, request.WorkSpaceType
                );

                if (overlappingOpenSpace)
                {
                    return (false, "Selected time is not available.", null, startDateTime, endDateTime);
                }

                await repository.OpenSpaceDescsChange(request.WorkSpaceType, true);
            }


            var room = await repository.GetRoomsByWorkspaceAndCapacity(request.WorkSpaceType, request.RoomCapacity);
            if (room == null || room.Quantity <= 0)
            {
                return (false, "There are no rooms left.", null, startDateTime, endDateTime);
            }

            bool overlapping = await repository.IsTimeOverlappingAsync(
                startDateTime, endDateTime, request.StartDate,
                request.RoomCapacity, request.WorkSpaceType
            );

            if (overlapping)
            {
                return (false, "Selected time is not available.", null, startDateTime, endDateTime);
            }

            return (true, "Valid", room, startDateTime, endDateTime);
        }
    }
}
