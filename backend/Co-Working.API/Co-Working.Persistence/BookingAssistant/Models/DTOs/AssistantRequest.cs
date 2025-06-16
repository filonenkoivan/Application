using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.BookingAssistant.Models.DTOs
{
    public class AssistantRequest
    {
        public string? model { get; set; }

        public AssistantMessage[] messages { get; set; }

        public AssistantRequest(string model, AssistantMessage[] message)
        {
            this.model = model;
            this.messages = message;
        }
    }
}
