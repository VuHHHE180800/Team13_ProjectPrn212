using System;
using System.Collections.Generic;

namespace BusinessModel;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportName { get; set; } = null!;

    public DateOnly ReportDate { get; set; }

    public int TotalAmount { get; set; }
}
