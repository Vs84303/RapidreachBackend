using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RapidReachNET.Models;

public partial class Tracking
{
    public long Id { get; set; }

    public DateOnly? CourierDate { get; set; }

    public string? TrackingStatus { get; set; }
    public long CourierId { get; set; }
    [JsonIgnore]

    public virtual Courier Courier { get; set; } = null!;
}
