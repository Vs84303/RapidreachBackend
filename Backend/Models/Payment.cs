using System;
using System.Collections.Generic;

namespace RapidReachNET.Models;

public partial class Payment
{
    public long Id { get; set; }

    public double? Amount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public long CourierId { get; set; }

    public virtual Courier Courier { get; set; } = null!;
}
