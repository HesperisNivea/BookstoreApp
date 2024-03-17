using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Born { get; set; }

    public DateOnly? Died { get; set; }

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();
}
