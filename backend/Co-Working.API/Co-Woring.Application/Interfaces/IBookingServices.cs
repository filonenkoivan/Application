﻿using Co_Woring.Application.DTOs;
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
    public interface IBookingServices
    {
        public Task<(bool Succes, string Text)> CreateAsync(BookingRequest request);

        public Task<List<WorkspaceResponse>> GetWorkspacesAsync();

        public Task<List<BookingResponse>> GetBookings();

        public Task<Response<string>> DeleteBooking(int id);
        public Task<BookingResponse> GetBookingAsync(int id);

        public Task<List<RoomDTO>> GetRoomsByType(WorkSpaceType type);
        public Task<(bool Success, string Message)> UpdateAsync(int id, BookingRequest request);
    }
}
