using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Bookings;
using Co_Woring.Application.DTOs.Coworking;
using Co_Woring.Application.DTOs.Enums;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Co_Working.API.Validation;
using Co_Working.Domain.Entities;
using Co_Working.Domain.Enums;
using DotNetEnv;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Co_Working.API.Endpoints
{
    public static class BookingEndpoints
    {
        public static void MapBookingEndpoints(this IEndpointRouteBuilder builder)
        {
            var bookings = builder.MapGroup("/bookings");

            bookings.MapPost("", CreateBooking);

            bookings.MapGet("/all", GetBookings);

            bookings.MapDelete("/{id}", DeleteBooking);

            bookings.MapGet("/{id}", GetBooking);

            bookings.MapPut("/{id}", UpdateBooking);


            bookings.MapGet("/rooms", GetRoomsByType);

            bookings.MapGet("/available", GetBookingsRooms);

            bookings.MapGet("/desk/{id}", GetBookingsDesks);

            bookings.MapGet("/exists", CheckBookingExists);

        }

        public async static Task<IResult> CreateBooking(BookingRequest request, IBookingServices services)
        {

            var validateResult = ValidationBooking.ValidateBooking(request);

            if (validateResult.Count > 0)
            {
                return Results.BadRequest(new Response<Dictionary<string, string[]>>(ApiStatusCode.BadRequest, "Validation problem", validateResult));
            }
            if (request.RoomCapacity != 0 && request.WorkSpaceType == WorkSpaceType.OpenSpace)
            {
                request.RoomCapacity = 0;
            }

            var result = await services.CreateAsync(request);
            return result.Success
                ? Results.Ok(new Response<string>(ApiStatusCode.Created, result.Text))
                : Results.BadRequest(new Response<string>(ApiStatusCode.BadRequest, result.Text, "Please choose a different time slot"));
        }
        public async static Task<Response<List<BookingResponse>>> GetBookings(IBookingServices services, HttpContext context)
        {
            int id = int.Parse(context.Request.Cookies["id"]);

            List<BookingResponse> result = await services.GetBookings(id);
            return new Response<List<BookingResponse>>(ApiStatusCode.Created, "Created", result);
        }
        public async static Task<Response<string>> DeleteBooking(IBookingServices services, int id)
        {
            return await services.DeleteBooking(id);
        }
        public async static Task<Response<BookingResponse>> GetBooking(int id, IBookingServices services)
        {
            return new Response<BookingResponse>(ApiStatusCode.Success, "Returned", await services.GetBookingAsync(id));
        }
        public async static Task<IResult> UpdateBooking(BookingRequest request, int id, IBookingServices services)
        {

            var validateResult = ValidationBooking.ValidateBooking(request);

            if (validateResult.Count > 0)
            {
                return Results.BadRequest(new Response<Dictionary<string, string[]>>(ApiStatusCode.BadRequest, "Validation problem", validateResult));
            }

            var result = await services.UpdateAsync(request: request, id: id);
            return result.Success
                ? Results.Ok(new Response<string>(ApiStatusCode.Created, result.Message))
                : Results.BadRequest(new Response<string>(ApiStatusCode.BadRequest, result.Message, "Please choose a different time slot"));

        }
        public async static Task<IResult> GetRoomsByType([FromQuery] WorkSpaceType type, IBookingServices services)
        {
            if (type != WorkSpaceType.OpenSpace)
            {
                return Results.Ok(await services.GetRoomsByType(type));
            }

            return Results.Ok(await services.GetDesksByType(type));
        }
        public async static Task<Response<List<BookingAvailableResponse>>> GetBookingsRooms([FromQuery] WorkSpaceType type, [FromQuery] int roomCapacity, IBookingServices services, int coworkingId)
        {
            var result = await services.GetBookingsByType(type, roomCapacity, coworkingId);
            return new Response<List<BookingAvailableResponse>>(ApiStatusCode.Success, "Returned", await services.GetBookingsByType(type, roomCapacity, coworkingId));
        }
        public async static Task<Response<List<BookingAvailableResponse>>> GetBookingsDesks(int id, IBookingServices services, int coworkingId)
        {
            var result = await services.GetBookingsDesks(id, coworkingId);
            return new Response<List<BookingAvailableResponse>>(ApiStatusCode.Success, "Returned", await services.GetBookingsDesks(id, coworkingId));
        }
        public async static Task<BookingExistsResponse> CheckBookingExists([FromQuery] WorkSpaceType workspaceId, int sessionId, IBookingServices services, int coworkingId)
        {
            var booking = await services.GetBookingByWorkspaceAndSessionIdAsync(workspaceId, sessionId, coworkingId);


            return booking;
        }
    }
}




