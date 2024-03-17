using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Interfaces;
using Labb2_DbFirst_Template.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb2_DbFirst_Template.Repositories;

public class BookRepository : IRepository<Book, string>
{
    private readonly BookstoreDbErDiagramContext _context;

    public BookRepository()
    {
        _context = BookstoreDbErDiagramContext.GetInstance();
    }

    public Book GetById(string id)
    {
        return _context.Books.Find(id);
        
    }

    public IEnumerable<Book> GetAll()
    {
        return _context.Books.Include(b => b.Series).Include(b => b.Authors);
    }

    public void Add(Book entity)
    {
        var book = entity as Book;

        if (entity != null)
        {
            _context.Books.Add(new Book()
            {
                Id = book.Id,
                Title = book.Title,
                Language = book.Language,
                Published = book.Published,
                Publisher = book.Publisher,
                Price = book.Price,
            });
            _context.SaveChanges();

        }
    }

    public void Update(Book entity)
    {
        if (entity != null)
        {
            var existingBook = _context.Books.Find(entity.Id);

            existingBook.Title = entity.Title;
            existingBook.Language = entity.Language;
            existingBook.Published = entity.Published;
            existingBook.Publisher = entity.Publisher;
            existingBook.Price = entity.Price;
            existingBook.Translator = entity.Translator;
            _context.SaveChanges();
        }
    }

    public void Delete(Book entity)
    {
        if (entity != null)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}