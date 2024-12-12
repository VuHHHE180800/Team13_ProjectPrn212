using System;
using System.Collections.Generic;

namespace Team13_ProjectPrn212.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly Dob { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public int RoleId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
