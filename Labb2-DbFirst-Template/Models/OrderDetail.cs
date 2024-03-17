using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
