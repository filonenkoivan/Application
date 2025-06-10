using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Bookings;
using Co_Woring.Application.DTOs.Desks;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using Co_Working.Domain.Interfaces;
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
        public async Task<(bool Success, string Text)> CreateAsync(BookingRequest request)
        {
            var validation = await ValidateBookingRequest(request);
            if (!validation.Success)
                return (false, validation.Message);

            var booking = new Booking
            {
                Name = request.Name,
                Email = request.Email,
                StartDateTime = validation.Start,
                EndDateTime = validation.End,
                SessionId = request.SessionId,
                WorkSpaceType = request.WorkSpaceType,
                RoomCapacity = request.RoomCapacity,
                DeskNumber = request.DeskNumber
            };

            if (validation.bookable is Desk)
            {
                booking.Desk = (Desk)validation.bookable;
            }
            else if (validation.bookable is Room)
            {
                booking.Room = (Room)validation.bookable;
            }
            await repository.AddAsync(booking);

            var message = booking.RoomCapacity == 0
            ? $"Your desk is booked from "
            : $"Your room for {booking.RoomCapacity} people is booked from";
            CultureInfo enCulture = new CultureInfo("en-US");

            return (true, $"{message} " +
                          $"{booking.StartDateTime.ToString("MMMM d, yyyy", enCulture)} to " +
                          $"{booking.EndDateTime.ToString("MMMM d, yyyy", enCulture)}. " +
                          $"Confirmation sent to {booking.Email}");
        }
        public async Task<(bool Success, string Message)> UpdateAsync(int id, BookingRequest request)
        {
            var existingBooking = await repository.GetBookingEntityAsync(id);
            if (existingBooking == null)
                return (false, "Booking not found");


            var validation = await ValidateBookingRequest(request, excludeBookingId: id);
            if (!validation.Success)
                return (false, validation.Message);

            if (validation.bookable is Desk desk)
            {
                existingBooking.Desk = desk;
                existingBooking.DeskNumber = request.DeskNumber;
                existingBooking.Room = null;
                existingBooking.RoomCapacity = 0;
            }
            else if (validation.bookable is Room room)
            {
                existingBooking.Room = room;
                existingBooking.RoomCapacity = request.RoomCapacity;
                existingBooking.Desk = null;
                existingBooking.DeskNumber = 0;
            }

            existingBooking.Name = request.Name;
            existingBooking.Email = request.Email;
            existingBooking.StartDateTime = validation.Start;
            existingBooking.EndDateTime = validation.End;
            existingBooking.WorkSpaceType = request.WorkSpaceType;

            await repository.UpdateAsync(existingBooking);
            await repository.DecreaseAvailabilityAsync(existingBooking);

            var message = existingBooking.RoomCapacity == 0
                ? $"Your desk is booked from "
                : $"Your room for {existingBooking.RoomCapacity} people is booked from";
            CultureInfo enCulture = new CultureInfo("en-US");

            return (true, $"{message} " +
                          $"{existingBooking.StartDateTime.ToString("MMMM d, yyyy", enCulture)} to " +
                          $"{existingBooking.EndDateTime.ToString("MMMM d, yyyy", enCulture)}. " +
                          $"Confirmation sent to {existingBooking.Email}");
        }
        public async Task<Response<string>> DeleteBooking(int id)
        {
            var result = await repository.DeleteBooking(id);

            if (result != true)
            {
                return new Response<string>(DTOs.Enums.ApiStatusCode.NotFound, "Booking not found");
            }
            else
            {
                return new Response<string>(DTOs.Enums.ApiStatusCode.Success, "Booking deleted");
            }
        }
        private async Task<(bool Success, string Message, IBookable bookable, DateTime Start, DateTime End)>
        ValidateBookingRequest(BookingRequest request, int? excludeBookingId = null)
        {
            var startDateTime = request.StartDate.Date + request.StartTime;
            var endDateTime = request.EndDate.Date + request.EndTime;

            var room = await repository.GetBookableByWorkspaceAndCapacity(request.WorkSpaceType, request.RoomCapacity, request.DeskNumber);

            if (room is Room && room.Quantity <= 0)
                return (false, "There are no rooms left.", null, startDateTime, endDateTime);

            if (room is Desk && room == null)
                return (false, "There are no desks left.", null, startDateTime, endDateTime);


            bool overlapping = await repository.IsTimeOverlappingAsync(
                startDateTime, endDateTime,
                request.RoomCapacity, request.WorkSpaceType,
                request.DeskNumber
            );

            if (overlapping)
            {
                return (false, "Selected time is not available.", null, startDateTime, endDateTime);
            }

            bool alreadyBooked = await repository.ExistsBookingWithSessionIdAndWorkspaceTypeAsync(
            request.SessionId,
            request.WorkSpaceType);

            if (alreadyBooked)
            {
                return (false, "You have already booked this type of workspace", null, startDateTime, endDateTime);
            }



            return (true, "Valid", room, startDateTime, endDateTime);
        }

        public async Task<List<BookingAvailableResponse>> GetBookingsByType(WorkSpaceType type, int capacity)
        {
            return await repository.GetBookingsByType(type, capacity);
        }
        public async Task<List<BookingAvailableResponse>> GetBookingsDesks(int deskId)
        {
            return await repository.GetBookingsDesks(deskId);
        }
        public async Task<List<BookingResponse>> GetBookings()
        {
            return await repository.GetBookingsAsync();
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
        public async Task<List<DeskDTO>> GetDesksByType(WorkSpaceType type)
        {
            return await repository.GetDesksByType(type);
        }

        public async Task<List<DeskDTO>> GetBookingByWorkspaceAndSessionIdAsync(WorkSpaceType type)
        {
            return await repository.GetDesksByType(type);
        }

        public async Task<BookingExistsResponse> GetBookingByWorkspaceAndSessionIdAsync(WorkSpaceType type, int id)
        {
            return await repository.GetBookingByWorkspaceAndSessionIdAsync(type, id);
        }
    }
}

