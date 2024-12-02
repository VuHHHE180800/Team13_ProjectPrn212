using System;
using System.Collections.Generic;

namespace Team13_ProjectPrn212.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int TotalPrice { get; set; }

    public int TotalQuantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
