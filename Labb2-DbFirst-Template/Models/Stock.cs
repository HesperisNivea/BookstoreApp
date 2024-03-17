using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class Stock 
{
    public int ShopId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int? Amount { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Shop Shop { get; set; } = null!;
}
