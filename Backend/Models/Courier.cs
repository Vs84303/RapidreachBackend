using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RapidReachNET.Models;

public partial class Courier
{
    public long Id { get; set; }

    public string? DropLocation { get; set; }

    public string? ParcelDescription { get; set; }

    public double? ParcelWeight { get; set; }

    public string? PickUpLocation { get; set; }

    public double? Price { get; set; }

    public long BranchId { get; set; }

    public long UserId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    
    public virtual ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();

    public virtual User User { get; set; } = null!;
}
