using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class VTitlesPerAuthor
{
    public int? NumberOfBooks { get; set; }

    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public decimal? TolatPriceForAllbooksInStock { get; set; }
}
