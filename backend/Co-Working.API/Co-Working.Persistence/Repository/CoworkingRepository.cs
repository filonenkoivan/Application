using Co_Woring.Application.DTOs.Coworking;
using Co_Woring.Application.Interfaces;
using Co_Working.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.Repository
{
    public class CoworkingRepository(AppDbContext db) : ICoworkingRepository
    {
        public List<CoworkingResponse> GetCoworkings()
        {

            return db.Coworkings
                .Include(x => x.Workspaces)
                .ThenInclude(x => x.AvailabilityDesks)
                .Include(x => x.Workspaces)
                .ThenInclude(x => x.AvailabilityRooms)
                .Select(x => new
                {
                    x.Id,
                    x.Workspaces,
                    x.Address,
                    x.Description,
                    x.PhotoUrl,
                    x.Name,
                }).AsEnumerable().Select(x => new CoworkingResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Address = x.Address,
                    PhotoUrl = x.PhotoUrl,
                    Desks = x.Workspaces.Where(x => x.WorkSpaceType == WorkSpaceType.OpenSpace).SelectMany(x => x.AvailabilityDesks).Sum(x => x.Quantity),
                    MeetingsRooms = x.Workspaces.Where(x => x.WorkSpaceType == WorkSpaceType.MeetingRoom).SelectMany(x => x.AvailabilityRooms).Sum(x => x.Quantity),
                    PrivateRooms = x.Workspaces.Where(x => x.WorkSpaceType == WorkSpaceType.PrivateRoom).SelectMany(x => x.AvailabilityRooms).Sum(x => x.Quantity),
                }).ToList();
        }
    }
}
