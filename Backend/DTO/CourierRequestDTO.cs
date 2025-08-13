namespace RapidReachNET.DTO
{
    public class CourierRequestDTO
    {
        public string DropLocation { get; set; }
        public string ParcelDescription { get; set; }
        public double ParcelWeight { get; set; }
        public string PickUpLocation { get; set; }
        public double Price { get; set; }
        public long BranchId { get; set; }
    }
}
