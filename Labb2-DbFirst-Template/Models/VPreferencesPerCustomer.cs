using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class VPreferencesPerCustomer
{
    public string FullName { get; set; } = null!;

    public string? MostBoughtSerie { get; set; }

    public int? TotalQuantityOfBooksFromSerie { get; set; }

    public int? NumberOfOrders { get; set; }

    public decimal? TotalPrice { get; set; }
}
