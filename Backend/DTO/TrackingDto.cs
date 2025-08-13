namespace RapidReachNET.DTO
{
    public class TrackingDto
    {
        public long Id { get; set; }
        public string? Status { get; set; }
        public DateOnly? UpdatedAt { get; set; }
    }
}
