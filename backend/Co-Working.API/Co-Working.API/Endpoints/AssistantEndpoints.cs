
using Co_Woring.Application.DTOs;
using Co_Woring.Application.DTOs.Enums;
using Co_Working.API.Contracts;
using Co_Working.Persistence.BookingAssistant.Models.DTOs;
using Co_Working.Persistence.BookingAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace Co_Working.API.Endpoints
{
    public static class AssistantEndpoints
    {
        public static void MapAssistantEndPoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/assistant", CreateRequest);
        }

        public static async Task<Response<string>> CreateRequest(BookingAssistant assistant, HttpContext context, AssistantContract request)
        {
            var id = int.Parse(context.Request.Cookies["id"]);
            var result = await assistant.SendRequest(request.Message, id);

            if (result == null)
            {
                return new Response<string>(statusCodes: ApiStatusCode.BadRequest, "Server error");
            }
            return new Response<string>(statusCodes: ApiStatusCode.Success, "Returned", result);
        }
    }
}
