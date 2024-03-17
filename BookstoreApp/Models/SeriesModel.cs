using Labb2_DbFirst_Template.Models;

namespace BookstoreApp.Models;

public class SeriesModel
{
    public int Id { get; set; }

    public string SeriesName { get; set; } = null!;

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();

}