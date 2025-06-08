using Co_Woring.Application.DTOs.Booking;
using Co_Working.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Co_Working.API.Validation
{
    public static class ValidationBooking
    {
        public static Dictionary<string, string[]> ValidateBooking(BookingRequest request)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);
            var validationErrors = new Dictionary<string, string[]>();

            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                validationErrors = validationResults.ToDictionary(
                    r => r.MemberNames.FirstOrDefault() ?? "",
                    r => new[] { r.ErrorMessage ?? "Invalid value" }
                );
            }

            var startDate = request.StartDate + request.StartTime;
            var endDate = request.EndDate + request.EndTime;
            if (startDate > endDate)
            {
                validationErrors.Add("StartTime", new[] { "The start of the booking must be earlier than the end" });
            }
            if (request.WorkSpaceType != WorkSpaceType.OpenSpace && request.RoomCapacity == 0)
            {
                validationErrors.Add("RoomCapacity", new[] { "No room selected" });
            }

            return validationErrors;
        }
    }
}
