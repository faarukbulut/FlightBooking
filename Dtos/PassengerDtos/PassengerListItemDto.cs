namespace FlightBooking.Dtos.PassengerDtos
{
    public class PassengerListItemDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }            // Erkek / Kadın
        public string PassengerType { get; set; }     // Yetişkin / Çocuk / Bebek
        public string Pnr { get; set; }
        public string SeatNumber { get; set; }        // 12A
        public string CheckInStatus { get; set; }     // Checked-In / Not Checked
        public string PaymentStatus { get; set; }     // Paid / Pending / Failed
        public string TicketStatus { get; set; }      // Issued / Not Issued
        public string Phone { get; set; }
    }
}
