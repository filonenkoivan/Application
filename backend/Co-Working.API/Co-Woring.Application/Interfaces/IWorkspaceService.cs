using Co_Woring.Application.DTOs.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Interfaces
{
    public interface IWorkspaceService
    {
        Task<List<WorkspaceResponse>> GetWorkspacesAsync(int id);
    }
}
