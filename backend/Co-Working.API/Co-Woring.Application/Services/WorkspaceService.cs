using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Services
{
    public class WorkspaceService(IWorkspaceRepository repository) : IWorkspaceService
    {
        public async Task<List<WorkspaceResponse>> GetWorkspacesAsync(int id)
        {
            return await repository.GetWorkspacesAsync(id);
        }
    }
}
