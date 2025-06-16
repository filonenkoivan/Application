using Co_Woring.Application.DTOs.Coworking;
using Co_Woring.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Woring.Application.Services
{
    public class CoworkingService(ICoworkingRepository repository) : ICoworkingService
    {
        public List<CoworkingResponse> GetCoworkings()
        {
            return repository.GetCoworkings();
        }
    }
}
