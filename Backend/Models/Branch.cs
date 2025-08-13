using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RapidReachNET.Models
{
    public partial class Branch
    {
        public long Id { get; set; }

        public string? Address { get; set; }

        public string? BranchName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        // One branch has many couriers
        [JsonIgnore]
        public virtual ICollection<Courier> Couriers { get; set; } = new List<Courier>();

        // One branch has many employees (users)
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
