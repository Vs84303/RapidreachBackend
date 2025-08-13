namespace RapidReachNET.DTO
{
    public class PaymentDto
    {

        public long Id { get; set; }
        public double? Amount { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public long CourierId { get; set; }
        public CourierDto? Courier { get; set; } 
    }
}
