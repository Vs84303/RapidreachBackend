using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RapidReachNET.Models
{
    public partial class User
    {
        public long UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Contact { get; set; }

        public string? Address { get; set; }

        public string? Pincode { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }

        public long? BranchId { get; set; }  // FK to Branch, nullable if user not assigned to any branch
        [JsonIgnore]
        public virtual Branch? Branch { get; set; }  // Navigation property

        public virtual ICollection<Courier> Couriers { get; set; } = new List<Courier>();

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
