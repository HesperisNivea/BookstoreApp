using System.Windows.Navigation;
using BookstoreApp.Models;
using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;

namespace BookstoreApp.Services;

public class BookService
{
    private readonly BookRepository _bookRepository = new ();

    public List<BookModel> GetAll()
    {
        return _bookRepository.GetAll()
            .Select(book => new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                Language = book.Language,
                Price = book.Price,
                Publisher = book.Publisher,
                Translator = book.Translator,
                Published = book.Published,
                Series = book.Series.Select(
                    series => new SeriesModel()
                    {
                        Id = series.Id,
                        SeriesName = series.SeriesName
                    }).ToList(),
                Authors = book.Authors.Select(
                    author => new AuthorModel()
                    {
                        Id = author.Id,
                        FirstName = author.FirstName,
                        LastName = author.LastName,
                        Born = author.Born,
                        Died = author.Died
                    }).ToList()

            }).ToList();
    }

    public void Update(BookModel book)
    {
        _bookRepository.Update(new Book()
        {
            Id = book.Id,
            Title = book.Title,
            Language = book.Language,
            Price = book.Price,
            Publisher = book.Publisher,
            Translator = book.Translator,
            Published = book.Published,
            Authors = book.Authors.Select(author => new Author()
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                Born = author.Born,
                Died = author.Died
            }).ToList()
        });

    }

    public void Add(BookModel book)
    {
        _bookRepository.Add(new Book()
        {
            Id = book.Id,
            Title = book.Title,
            Language = book.Language,
            Price = book.Price,
            Publisher = book.Publisher,
            Translator = book.Translator,
            Published = book.Published,
            Authors = book.Authors.Select(author => new Author()
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                Born = author.Born,
                Died = author.Died
            }).ToList()
        });
    }

    public void Remove(BookModel book)
    {
        var temp = _bookRepository.GetById(book.Id);
        _bookRepository.Delete(temp);
    }
}