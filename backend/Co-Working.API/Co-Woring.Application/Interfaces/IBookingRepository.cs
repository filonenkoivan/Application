using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Bookings;
using Co_Woring.Application.DTOs.Desks;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using Co_Working.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime, int roomCapacity, WorkSpaceType workSpaceType, int deskNumber);
        Task<List<WorkspaceResponse>> GetWorkspacesAsync();
        Task<List<BookingResponse>> GetBookingsAsync();
        Task<bool> DeleteBooking(int id);
        Task<IBookable> GetBookableByWorkspaceAndCapacity(WorkSpaceType type, int capacity, int deskNumber);
        Task<BookingResponse> GetBookingAsync(int id);
        Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type);
        Task<List<DeskDTO>> GetDesksByType(WorkSpaceType type);
        Task UpdateAsync(Booking booking);
        Task<Booking> GetBookingEntityAsync(int id);
        Task DecreaseAvailabilityAsync(Booking booking);
        Task<List<BookingAvailableResponse>> GetBookingsByType(WorkSpaceType type, int capacity);
        Task<List<BookingAvailableResponse>> GetBookingsDesks(int deskId);

        Task<bool> ExistsBookingWithSessionIdAndWorkspaceTypeAsync(int sessionId, WorkSpaceType workSpaceType);
        Task<BookingExistsResponse> GetBookingByWorkspaceAndSessionIdAsync(WorkSpaceType type, int id);
    }
}
