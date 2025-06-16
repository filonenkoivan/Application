using Co_Woring.Application.DTOs.Coworking;
using Co_Woring.Application.DTOs.Enums;
using Co_Woring.Application.DTOs;
using Co_Woring.Application.Interfaces;

namespace Co_Working.API.Endpoints
{
    public static class CoworkingEndpoints
    {
        public static void MapCoworkingEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/coworking", GetCoworkings);
        }

        public async static Task<Response<List<CoworkingResponse>>> GetCoworkings(ICoworkingService service)
        {
            return new Response<List<CoworkingResponse>>(ApiStatusCode.Success, "Returned", service.GetCoworkings());
        }
    }
}
