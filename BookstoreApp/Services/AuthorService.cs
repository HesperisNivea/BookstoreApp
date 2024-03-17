using BookstoreApp.Models;
using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;

namespace BookstoreApp.Services;

public class AuthorService
{
    private readonly AuthorRepository _authorRepository = new();

    private readonly BookRepository _bookRepository = new BookRepository();
    public List<AuthorModel> GetAll()
    {
        return _authorRepository.GetAll().Select(
            author => new AuthorModel()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Born = author.Born,
                Died = author.Died
            }).ToList();
    }

    public void Add(AuthorModel author)
    {
        _authorRepository.Add(new Author()
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            Born = author.Born,
            Died = author.Died
        });
    }

    public void Update(AuthorModel author)
    {
        _authorRepository.Update(new Author()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Born = author.Born,
            Died = author.Died
        });
    }

    public void Remove(AuthorModel author)
    {
        var temp = _authorRepository.GetById(author.Id);
        _authorRepository.Delete(temp);
    }

    public void AddBook(BookModel book)
    {
        var existingBook = _bookRepository.GetById(book.Id);
        Author author = null;

        if (book.Authors.Count > 0)
        {
            foreach (var bookAuthor in book.Authors)
            {
                author = _authorRepository.GetById(bookAuthor.Id);
                author.Isbn13s.Add(_bookRepository.GetById(book.Id));
                _authorRepository.Update(author);
            }

        }

    }

    public void UpdateBook(BookModel book) // bookModel has isbn13s list with info about removed(false) and added(true) authors. This way it is possible to remove books from authors lists.
    {
        var existingBook = _bookRepository.GetById(book.Id);
        Author author = null;

        if (book.Authors.Count > 0)
        {
            foreach (var authorModel in book.Authors)
            {
                author = _authorRepository.GetById(authorModel.Id);
                if (author != null)
                {
                    if (authorModel.IsSelected == false)
                    {
                        author.Isbn13s.Remove(existingBook);
                    }
                    else if (authorModel.IsSelected == true)
                    {
                        author.Isbn13s.Add(_bookRepository.GetById(book.Id));
                    }

                    _authorRepository.Update(author);
                }



            }
        }

    }

    public void RemoveBook(BookModel book)
    {
        var existingBook = _bookRepository.GetById(book.Id);
        Author author = null;

        if (book.Authors.Count > 0)
        {
            foreach (var authorModel in book.Authors)
            {
                author = _authorRepository.GetById(authorModel.Id);
                if (author != null)
                {
                    author.Isbn13s.Remove(existingBook);
                    _authorRepository.Update(author);
                }
            }
        }
    }
}