namespace AirportTicketBookingSystem.Domain.Massages
{
    public static class FileHeaders
    {
        public const string BookingHeader = "Id,FlightId,PassengerId,PassengerName,PassengerEmail,Class,BookDate";
        public const string FlightHeader = "Id,DepartureCountry,DestinationCountry,DepartureDate,DepartureAirport,ArrivalAirport,Prices";
    }
}
