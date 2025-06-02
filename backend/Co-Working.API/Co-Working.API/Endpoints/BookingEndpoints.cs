using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Booking;
using Co_Woring.Application.DTOs.Enums;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
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
            builder.MapPost("/bookings", CreateBooking);

            builder.MapGet("/bookings", GetBookings);

            builder.MapDelete("/bookings/{id}", DeleteBooking);

            builder.MapGet("/bookings/{id}", GetBooking);

            builder.MapPut("/bookings/{id}", UpdateBooking);
            // get all available workspaces on the home page
            builder.MapGet("/workspaces", GetWorkspaces);

            builder.MapGet("/bookings/rooms", GetRoomsByType);
        }

        public async static Task<IResult> CreateBooking(BookingRequest request, IBookingServices services)
        {

            var validateResult = ValidateBooking(request);
            if (validateResult.Count > 0)
            {
                Results.BadRequest(new Response<Dictionary<string, string[]>>(ApiStatusCode.BadRequest, "Validation problem", validateResult));
            }

            var result = await services.CreateAsync(request);
            return result.Succes
                ? Results.BadRequest(new Response<string>(ApiStatusCode.BadRequest, result.Text, "Please choose a different time slot"))
                : Results.Ok(new Response<string>(ApiStatusCode.Created, result.Text));
        }
        public async static Task<Response<List<BookingResponse>>> GetBookings(IBookingServices services)
        {
            List<BookingResponse> result = await services.GetBookings();
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
        public async static Task UpdateBooking(BookingRequest request, int id, IBookingServices services)
        {
            await services.UpdateAsync(request: request, id: id);
        }
        /////////////////
        public static Dictionary<string, string[]> ValidateBooking(BookingRequest request)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);
            var validationErrors = new Dictionary<string, string[]>();

            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                validationErrors = validationResults.ToDictionary(
                    r => r.MemberNames.FirstOrDefault() ?? "",
                    r => new[] { r.ErrorMessage ?? "Invalid value" }
                );
            }

            var startDate = request.StartDate + request.StartTime;
            var endDate = request.EndDate + request.EndTime;
            if (startDate > endDate)
            {
                validationErrors.Add("StartTime", new[] { "The start of the booking must be earlier than the end" });
            }

            return validationErrors;
        }




        public async static Task<Response<List<WorkspaceResponse>>> GetWorkspaces(IBookingServices services)
        {
            List<WorkspaceResponse> result = await services.GetWorkspacesAsync();

            return new Response<List<WorkspaceResponse>>(ApiStatusCode.Created, "Created", result);
        }
        public async static Task<List<RoomDTO>> GetRoomsByType([FromQuery] WorkSpaceType type, IBookingServices services)
        {
            return await services.GetRoomsByType(type);
        }
    }
}

