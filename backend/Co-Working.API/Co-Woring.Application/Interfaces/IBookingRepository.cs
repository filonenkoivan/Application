using Co_Woring.Application.DTOs.Booking;
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
    public interface IBookingRepository
    {
        public Task<Booking> AddAsync(Booking booking);
        public Task<bool> IsTimeOverlappingAsync(DateTime startDateTime, DateTime endDateTime, DateTime startDate, int roomCapacity, WorkSpaceType workSpaceType);
        public Task<List<WorkspaceResponse>> GetWorkspacesAsync();
        public Task<List<BookingResponse>> GetBookingsAsync();
        public Task<bool> DeleteBooking(int id);

        public Task<Room> GetRoomsByWorkspaceAndCapacity(WorkSpaceType type, int capacity);

        public Task<BookingResponse> GetBookingAsync(int id);
        public Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type);

        public Task UpdateAsync(Booking booking);

        public Task<Booking> GetBookingEntityAsync(int id);

        public Task OpenSpaceDescsChange(WorkSpaceType workspace, bool increment);

    }
}
