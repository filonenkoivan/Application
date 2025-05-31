using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Working.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Interfaces
{
    public interface IBookingServices
    {
        public Task CreateAsync(BookingRequest request);

        public Task<List<WorkspaceResponse>> GetWorkspacesAsync();

        public Task<List<BookingResponse>> GetBookings();

        public Task DeleteBooking(int id);
        public Task UpdateBooking(int id, BookingRequest request);
    }
}
