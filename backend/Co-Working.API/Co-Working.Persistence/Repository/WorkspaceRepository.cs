using Co_Woring.Application.DTOs.Desks;
using Co_Woring.Application.DTOs.Rooms;
using Co_Woring.Application.DTOs.Workspaces;
using Co_Woring.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.Repository
{
    public class WorkspaceRepository(AppDbContext db) : IWorkspaceRepository
    {
        public async Task<List<WorkspaceResponse>> GetWorkspacesAsync(int id)
        {
            var currentCoworking = await db.Coworkings
                .Include(x => x.Workspaces)
                .ThenInclude(x => x.AvailabilityDesks)
                .Include(x => x.Workspaces)
                .ThenInclude(x => x.AvailabilityRooms)
                .FirstOrDefaultAsync(x => x.Id == id);
            return currentCoworking.Workspaces.Select(x => new WorkspaceResponse
            {
                Name = x.Name,
                Description = x.Description,
                WorkSpaceType = x.WorkSpaceType,
                Id = x.Id,
                AvailabilityRooms = x.AvailabilityRooms
                    .Select(y => new RoomDTO
                    {
                        Capacity = y.Capacity,
                        Id = y.Id,
                        Quantity = y.Quantity
                    }).ToList(),
                AvailabilityDesks = x.AvailabilityDesks
                    .Select(y => new DeskDTO
                    {
                        Id = y.Id,
                        Quantity = y.Quantity
                    }).ToList(),
                Amenities = x.Amenities,
                Capacity = x.Capacity,
                PhotoList = x.PhotoList,
            }).ToList();
        }
    }
}
