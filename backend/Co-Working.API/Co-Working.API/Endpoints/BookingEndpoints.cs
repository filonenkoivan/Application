using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;

namespace Co_Working.API.Endpoints
{
    public static class BookingEndpoints
    {
        public static void MapBookingEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/bookings", CreateBooking);

            builder.MapGet("/bookings", GetBookings);

            builder.MapDelete("/bookings/:id", DeleteBooking);
            //update if need
            builder.MapPost("/bookings/:id", UpdateBooking);

            builder.MapGet("/workspaces", GetWorkspaces);
        }

        public async static Task CreateBooking(BookingRequest request, IBookingServices services)
        {
            await services.CreateAsync(request);
            // Task<Response<BookingResponse>>
        }

        public async static Task<Response<List<WorkspaceResponse>>> GetWorkspaces(IBookingServices services)
        {
            List<WorkspaceResponse> result = await services.GetWorkspacesAsync();

            return new Response<List<WorkspaceResponse>>(Co_Woring.Application.DTOs.Enums.StatusCodes.Created, "created", result);
        }

        public async static Task<Response<List<BookingResponse>>> GetBookings(IBookingServices services)
        {
            List<BookingResponse> result = await services.GetBookings();
            return new Response<List<BookingResponse>>(Co_Woring.Application.DTOs.Enums.StatusCodes.Created, "created", result);
        }

        public async static Task DeleteBooking(IBookingServices services, int id)
        {
            await services.DeleteBooking(id);
            //return new Response<List<BookingResponse>>(Co_Woring.Application.DTOs.Enums.StatusCodes.Created, "created", result);
        }

        public async static Task UpdateBooking(IBookingServices services, int id, BookingRequest request)
        {
            await services.UpdateBooking(id, request);
            //return new Response<List<BookingResponse>>(Co_Woring.Application.DTOs.Enums.StatusCodes.Created, "created", result);
        }
    }
}

