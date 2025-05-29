using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Entities;

namespace Co_Working.API.Endpoints
{
    public static class BookingEndpoints
    {
        public static void MapBookingEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/bookings", CreateBooking);
        }

        public async static Task CreateBooking(BookingRequest request, IBookingServices services)
        {
            await services.CreateAsync(request);
            // Task<Response<BookingResponse>>
        }
    }
}
