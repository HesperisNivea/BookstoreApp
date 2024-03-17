using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class Series
{
    public int Id { get; set; }

    public string SeriesName { get; set; } = null!;

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();
}
