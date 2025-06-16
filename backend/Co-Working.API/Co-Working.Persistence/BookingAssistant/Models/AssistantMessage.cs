using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.BookingAssistant.Models
{
    public class AssistantMessage
    {
        public string? role { get; set; }
        public string? content { get; set; }

        public AssistantMessage(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
    }
}
