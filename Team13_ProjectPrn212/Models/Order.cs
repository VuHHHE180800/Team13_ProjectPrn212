using System;
using System.Collections.Generic;

namespace Team13_ProjectPrn212.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerAddress { get; set; }

    public string? CustomerPhonenumber { get; set; }

    public DateOnly OrderDate { get; set; }

    public int StatusId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus Status { get; set; } = null!;
}
