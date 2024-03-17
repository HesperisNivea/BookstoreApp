using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string County { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
