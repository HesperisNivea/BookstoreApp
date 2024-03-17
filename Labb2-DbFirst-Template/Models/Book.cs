using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class Book
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Language { get; set; }

    public decimal Price { get; set; }

    public string Publisher { get; set; } = null!;

    public string? Translator { get; set; }

    public DateOnly Published { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
