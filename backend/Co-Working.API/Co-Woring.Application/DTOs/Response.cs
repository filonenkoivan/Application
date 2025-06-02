using Co_Woring.Application.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Co_Woring.Application.DTOs
{
    public class Response<T>
    {
        public T Data { get; set; }
        public ApiStatusCode StatusCode { get; set; }
        public string? Message { get; set; }

        public Response(ApiStatusCode statusCodes, string message, T data = default)
        {
            Data = data;
            StatusCode = statusCodes;
            Message = message;
        }
    }
}
