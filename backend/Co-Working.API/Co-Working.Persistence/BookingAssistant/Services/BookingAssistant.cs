using Co_Woring.Application.Interfaces;
using Co_Working.Persistence.BookingAssistant.Models;
using Co_Working.Persistence.BookingAssistant.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Co_Working.Persistence.BookingAssistant.Services
{
    public class BookingAssistant(HttpClient client, IBookingRepository bookingRepository)
    {
        public async Task<string> SendRequest(string message, int id)
        {
            var bookings = JsonSerializer.Serialize(await bookingRepository.GetBookingsForAssitant(id));
            string prompt =
              "You are an assistant that helps users understand their workspace bookings.\n\n" +
              "Here is a list of the user's bookings:\n\n" +
              $"{bookings}\n\n" +
              $"The user asked: \"{message}\"\n\n" +
              "Your task:\n" +
              "- If the user asks about a specific day (e.g. \"May 12\"), reply with only whether they have bookings on that day — do NOT show bookings for other days\n" +
              "- Count bookings if the user asks \"how many\"\n" +
              "- List upcoming bookings if asked about \"next week\", \"future\", or similar\n" +
              "- Show bookings from last week\n" +
              "- Filter by type: private room, meeting room, or desk (workspace types are coded as: 0 - Open Space, 1 - Private Room, 2 - Meeting Room)\n" +
              "- If the user asks generally about their bookings, return a nicely formatted list of all their bookings\n" +
              "- If the question doesn’t make sense, reply: \"Sorry, I didn’t understand that. Please try rephrasing your question.\"\n\n" +
              "Format your response using basic HTML so it can be directly inserted into a webpage. Use:\n" +
              "- <strong> for emphasizing important parts (dates, workspace types, etc.)\n" +
              "- <br> for line breaks between messages and bookings\n" +
              "- For filtered lists (by type, date, etc.), start with a heading like:\n" +
              "  <div class='font-bold text-[24px] mb-6'>List all my private room bookings</div>\n" +
              "- Do not include phrases like \"You asked\" or \"Here is the answer\" — just start with the heading when appropriate\n" +
              "- Do not wrap the entire message in a paragraph tag\n\n" +
              "When listing bookings, follow this format:\n" +
              "📅 <strong>[Date in full, e.g. June 12, 2025]</strong> — <strong>[Workspace type (capitalized)]</strong> for [X] people at [Location] ([Start – End time])<br>\n\n" +
              "Example questions and responses:\n\n" +
              "User: Do I have anything on May 29?\n" +
              "Assistant:\n" +
              "You don’t have any bookings on <strong>May 29, 2025</strong>.<br>\n\n" +
              "User: List all my private room bookings\n" +
              "Assistant:\n" +
              "<div class='font-bold text-[24px] mb-6'>List all my private room bookings</div>" +
              "📅 <strong>June 20, 2025</strong> — <strong>Private Room</strong> for 3 people at UrbanSpace Podil (10:00 – 12:00)<br>" +
              "📅 <strong>June 25, 2025</strong> — <strong>Private Room</strong> for 2 people at Coworking Arena (09:00 – 11:00)<br>\n\n" +
              "Always be concise, accurate, and clearly structured.";


            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "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");
            var messageModel = new AssistantMessage("user", message);
            var assistantRequest = new AssistantRequest("llama-3.3-70b-versatile", new AssistantMessage[] { new AssistantMessage("user", prompt) });
            JsonContent content = JsonContent.Create(assistantRequest);
            var response = await client.PostAsJsonAsync("https://api.groq.com/openai/v1/chat/completions", assistantRequest);
            var final = JsonSerializer.Deserialize<AssistantResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return final.Choices.FirstOrDefault().Message.Content;

        }
    }
}
