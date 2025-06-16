using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Persistence.BookingAssistant.Models.DTOs
{
    public class AssistantResponse
    {
        public Choices[] Choices { get; set; }
    }

    public class Choices
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Content { get; set; }
    }
}

