using System;
using System.Collections.Generic;

namespace Team13_ProjectPrn212.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportName { get; set; } = null!;

    public DateOnly ReportDate { get; set; }

    public int TotalAmount { get; set; }
}
