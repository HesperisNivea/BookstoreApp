using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labb2_DbFirst_Template.Models;

namespace BookstoreApp.Models;

public class BookModel 
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Language { get; set; }

    public decimal Price { get; set; }

    public string Publisher { get; set; } = null!;

    public string? Translator { get; set; }

    public DateOnly Published { get; set; }

    public virtual List<AuthorModel> Authors { get; set; } = new ();

    public virtual List<SeriesModel> Series { get; set; } = new();

}