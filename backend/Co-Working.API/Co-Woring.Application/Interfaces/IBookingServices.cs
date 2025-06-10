using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Bookings;
using Co_Woring.Application.DTOs.Desks;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Interfaces
{
    public interface IBookingServices
    {
        Task<(bool Success, string Text)> CreateAsync(BookingRequest request);
        Task<List<WorkspaceResponse>> GetWorkspacesAsync();
        Task<List<BookingResponse>> GetBookings();
        Task<Response<string>> DeleteBooking(int id);
        Task<BookingResponse> GetBookingAsync(int id);
        Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type);
        Task<List<DeskDTO>> GetDesksByType(WorkSpaceType type);
        Task<(bool Success, string Message)> UpdateAsync(int id, BookingRequest request);
        Task<List<BookingAvailableResponse>> GetBookingsByType(WorkSpaceType type, int capacity);
        Task<List<BookingAvailableResponse>> GetBookingsDesks(int deskId);
        Task<BookingExistsResponse> GetBookingByWorkspaceAndSessionIdAsync(WorkSpaceType type, int id);
    }
}
