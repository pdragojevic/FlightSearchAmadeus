using System.ComponentModel.DataAnnotations;

namespace FlightSearch.Domain.Entities;

public class FlightsSearchParameters
{
    [Required(ErrorMessage = "Origin location code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Origin location code must be exactly 3 characters.")]
    public string OriginLocationCode { get; set; }

    [Required(ErrorMessage = "Destination location code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Destination location code must be exactly 3 characters.")]
    public string DestinationLocationCode { get; set; }

    [Required(ErrorMessage = "Departure date is required.")]
    [CustomValidation(typeof(FlightsSearchParameters), nameof(ValidateFutureDate))]
    public DateTime DepartureDate { get; set; }

    [CustomValidation(typeof(FlightsSearchParameters), nameof(ValidateReturnDate))]
    public DateTime? ReturnDate { get; set; }

    [Required(ErrorMessage = "Number of adults is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "The number of adults must be at least 1.")]
    public int Adults { get; set; }

    [Required(ErrorMessage = "Currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    public string CurrencyCode { get; set; }

    // Custom Validation Methods
    public static ValidationResult ValidateFutureDate(DateTime date, ValidationContext context)
    {
        if (date < DateTime.UtcNow.Date)
        {
            return new ValidationResult("Departure date must be in the future.");
        }
        return ValidationResult.Success;
    }

    public static ValidationResult ValidateReturnDate(DateTime? returnDate, ValidationContext context)
    {
        var instance = context.ObjectInstance as FlightsSearchParameters;
        if (returnDate.HasValue && instance != null && returnDate < instance.DepartureDate)
        {
            return new ValidationResult("Return date cannot be earlier than the departure date.");
        }
        return ValidationResult.Success;
    }
}