namespace RapidReachNET.DTO
{
    public class CourierDto
    {
        public long Id { get; set; }
        public string? DropLocation { get; set; }
        public string? ParcelDescription { get; set; }
        public double? ParcelWeight { get; set; }
        public string? PickUpLocation { get; set; }
        public double? Price { get; set; }
        public long BranchId { get; set; }
        public long UserId { get; set; }

        public UserDtoR? User { get; set; }

        public List<TrackingDto>? Trackings { get; set; }
    }
}
