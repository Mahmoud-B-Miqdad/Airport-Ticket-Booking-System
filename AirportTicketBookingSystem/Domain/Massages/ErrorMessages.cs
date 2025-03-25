namespace AirportTicketBookingSystem.Domain.Massages;

public static class ErrorMessages
{
    public const string BookingDataParseError = "Error parsing Booking data: {0}";
    public const string BookingAlreadyExists = "Booking {0} already exists";
    public const string BookingNotFound = "Booking ID {0} Not Found.";
    public const string ErrorLoadingBookings = "Error loading bookings: {0}";
    public const string InvalidPassengerID = "{0} Is Invalid passenger ID";

    public const string FlightDataFileNotFound = "Flight data file not found in Resources folder.";
    public const string InvalidDataColumnCount = "Invalid data at line {0}: Incorrect number of columns.";
    public const string InvalidData = "Invalid data at line {0}: {1}";
    public const string FileReadFailure = "Failed to read file: {0}";

    public const string FlightDataParsingError = "Error parsing flight data: {0}";
    public const string FlightLoadingError = "Error loading flights: {0}";
    public const string FlightAlreadyExists = "Flight {0} already exists";

    public const string FlightIdNullError = "flightId shouldn't be null";
    public const string BookingNotFoundError = "Booking with ID {0} not found.";
}
