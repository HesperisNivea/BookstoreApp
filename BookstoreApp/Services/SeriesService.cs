using System.Configuration;
using System.Runtime.Serialization;
using BookstoreApp.Models;
using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;

namespace BookstoreApp.Services;

public class SeriesService
{
    private readonly SeriesRepository _seriesRepository = new SeriesRepository();

    private readonly BookRepository _bookRepository = new BookRepository();

    public List<SeriesModel> GetAll()
    {
        return _seriesRepository.GetAll().Select(
            series => new SeriesModel()
            {
                Id = series.Id,
                SeriesName = series.SeriesName
            }).ToList();
    }

    public void AddBook(SeriesModel series, BookModel book)
    {
        // check if seriesModel id matches with id from database 

        // check if there is a book with matching isbn in series Collection 
        if (series == null)
        {
            return;
        }
        // update series collection or series object by update
        var serie = _seriesRepository.GetById(series.Id);
        if (serie != null)
        {
            serie.Isbn13s.Add(_bookRepository.GetById(book.Id));
            _seriesRepository.Update(serie);

        }



    }

    public void RemoveBook(BookModel book) // it has to go before Book removal 
    {
        var existingBook = _bookRepository.GetById(book.Id);
        Series series = null;

        if (book.Series.Count > 0) // it checks if there is a series that has same Id as SeriesModel because there is an default seriesModel with id = 0 (None)
        {
            series = _seriesRepository.GetById(book.Series.ToList()[0].Id);
        }

        if (series != null)
        {
            series.Isbn13s.Remove(existingBook);
        }

    }
}