using Co_Working.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Co_Woring.Application.DTOs.Coworking
{
    public class CoworkingResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? PhotoUrl { get; set; }
        public int Desks { get; set; }
        public int PrivateRooms { get; set; }
        public int MeetingsRooms { get; set; }
    }
}

