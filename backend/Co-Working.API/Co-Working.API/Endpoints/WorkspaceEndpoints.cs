using Co_Woring.Application.DTOs.Enums;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.DTOs;
using Co_Woring.Application.Interfaces;

namespace Co_Working.API.Endpoints
{
    public static class WorkspaceEndpoints
    {
        public static void MapWorkspaceEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/workspaces/{id}", GetWorkspaces);
        }

        public async static Task<Response<List<WorkspaceResponse>>> GetWorkspaces(int id, IWorkspaceService services)
        {
            List<WorkspaceResponse> result = await services.GetWorkspacesAsync(id);

            return new Response<List<WorkspaceResponse>>(ApiStatusCode.Created, "Returned", result);
        }
    }
}
