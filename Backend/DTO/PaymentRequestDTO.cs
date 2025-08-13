namespace RapidReachNET.DTO
{
    public class PaymentRequestDTO
    {
        public long CourierId { get; set; }
        public double Amount { get; set; }
        public DateOnly PaymentDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    }
}
