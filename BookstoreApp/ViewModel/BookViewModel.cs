using System.Collections.ObjectModel;
using System.Globalization;
using System.Printing;
using System.Security.RightsManagement;
using System.Text.RegularExpressions;
using System.Windows;
using BookstoreApp.Models;
using BookstoreApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_DbFirst_Template.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookstoreApp.ViewModel;

public class BookViewModel : ObservableObject
{
    private readonly BookService _bookService = new BookService();

    private readonly AuthorService _authorService = new AuthorService();

    private readonly SeriesService _seriesService = new SeriesService();

    private readonly StockService _stockService = new StockService();

    #region Properties

    private List<BookModel> _bookSelection;  
    public List<BookModel> BookSelection
    {
        get { return _bookSelection; }
        set
        {
            _bookSelection = value;
            OnPropertyChanged();
        }
    }

    private BookModel _selectedBook;

    public BookModel SelectedBook
    {
        get { return _selectedBook; }
        set
        {
            _selectedBook = value;
            if (_selectedBook != null)
            {
                PopulateBookInfo();
            }
            UpdateExistingBook.NotifyCanExecuteChanged();
            SaveNewBook.NotifyCanExecuteChanged();
            RemoveBook.NotifyCanExecuteChanged();
            OnPropertyChanged();
        }
    }


    private string _selectedTitle;

    public string SelectedTitle
    {
        get { return _selectedTitle; }
        set
        {
            _selectedTitle = value;
            OnPropertyChanged();
        }
    }

    private string _selectedLanguage;

    public string SelectedLanguage
    {
        get { return _selectedLanguage; }
        set
        {
            _selectedLanguage = value;
            OnPropertyChanged();
        }
    }

    private double _selectedPrice;

    public double SelectedPrice
    {
        get { return _selectedPrice; }
        set
        {
            _selectedPrice = value;
            OnPropertyChanged();
        }
    }

    private string _selectedPublisher;

    public string SelectedPublisher
    {
        get { return _selectedPublisher; }
        set
        {
            _selectedPublisher = value;
            OnPropertyChanged();
        }
    }

    private string _selectedTranslator;

    public string SelectedTranslator
    {
        get { return _selectedTranslator; }
        set
        {
            _selectedTranslator = value;
            OnPropertyChanged();
        }
    }

    private string _selectedId;

    public string SelectedId
    {
        get { return _selectedId; }
        set
        {
            _selectedId = value;
            UpdateExistingBook.NotifyCanExecuteChanged();
            OnPropertyChanged();
        }
    }

    private DateTime _selectedPublishDate;

    public DateTime SelectedPublishDate
    {
        get { return _selectedPublishDate; }
        set
        {
            _selectedPublishDate = value;
            OnPropertyChanged();
        }
    }
 
    private List<AuthorModel> _authorList;

    public List<AuthorModel> AuthorList
    {
        get { return _authorList; }
        set
        {
            _authorList = value;
            OnPropertyChanged();
        }
    }

    private List<AuthorModel> _selectedAuthors;

    public List<AuthorModel> SelectedAuthors // fix!!!
    {
        get { return _selectedAuthors; }
        set
        {
            _selectedAuthors = value;
            OnPropertyChanged();
        }
    }
   
    private List<SeriesModel> _seriesModelsList;

    public List<SeriesModel> SeriesModelsList
    {
        get { return _seriesModelsList; }
        set
        {
            _seriesModelsList = value;
            OnPropertyChanged();
        }
    }

    private SeriesModel _selectedSeries;

    public SeriesModel SelectedSeries
    {
        get
        {
            return _selectedSeries;
        }
        set
        {
            _selectedSeries = value;
            OnPropertyChanged();
        }
    }

    private int _selectedSeriesIndex;

    public int SelectedSeriesIndex
    {
        get { return _selectedSeriesIndex; }
        set
        {
            _selectedSeriesIndex = value;
            OnPropertyChanged();
        }
    }

    private bool  _isReadOnlyIsbnTxtBox = true;

    public bool IsReadOnlyIsbnTxtBox
    {
        get { return _isReadOnlyIsbnTxtBox; }
        set
        {
            _isReadOnlyIsbnTxtBox = value;
            OnPropertyChanged();
        }
    }
    #endregion

    private BookModel _defaultBook = new BookModel() { Title = "--New Book--" };

    private SeriesModel _defaultSerie = new SeriesModel() { SeriesName = "--None--" };

    public static Action BookSelectionIsChanged;

    public IRelayCommand SaveNewBook { get; }
    public IRelayCommand UpdateExistingBook { get; }
    public IRelayCommand RemoveBook { get; }
    public IRelayCommand FillInTextBoxWithInfoFromSelectedBook { get; }

    public BookViewModel()
    {
        UpdateBookSelection();
        _authorList = _authorService.GetAll();
        _seriesModelsList = _seriesService.GetAll();
        _seriesModelsList.Add(_defaultSerie);
        SaveNewBook = new RelayCommand(SaveNewBookExecuteCommand, SaveNewBookCommandCanExecute);
        UpdateExistingBook = new RelayCommand(UpdateExistingBookExecuteCommand, UpdateExistingBookCommandCanExecute);
        RemoveBook = new RelayCommand(RemoveBookExecuteCommand, RemoveBookCommandCanExecute);
        FillInTextBoxWithInfoFromSelectedBook = new RelayCommand(FillInTextBoxWithInfoFromSelectedBookExecuteCommand);
        AuthorViewModel.AuthorSelectionIsChanged += AuthorSelectionIsChanged;
    }

    private void AuthorSelectionIsChanged()
    {
        if (SelectedBook != null)
        {
            PopulateBookInfo();
        }
    }

    private void FillInTextBoxWithInfoFromSelectedBookExecuteCommand()
    {
        PopulateBookInfo();
    }

    private bool RemoveBookCommandCanExecute()
    {
        if (SelectedBook == null || SelectedBook.Id == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void RemoveBookExecuteCommand()
    {
        var stock = _stockService.GetAll();
        if (stock.All(s => s.Isbn13 != SelectedId))
        {
            _authorService.RemoveBook(SelectedBook);
            _seriesService.RemoveBook(SelectedBook);
            _bookService.Remove(SelectedBook);
        }
        else
        {
            MessageBox.Show(
                "This book is in stock in some store, before you can remove it, first remove it from stock!!");
        }
        
        UpdateBookSelection();
        BookSelectionIsChanged.Invoke();
    }

    private bool UpdateExistingBookCommandCanExecute()
    {
        if (SelectedBook == null)
        {
            return false;
        }
        else
        {
            if (SelectedBook.Id == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private void UpdateExistingBookExecuteCommand()
    {
        var bookUpdate = CollectDataAndFillBookModel();
        _seriesService.RemoveBook(SelectedBook);
        //Update relations between books and authors 
        _authorService.UpdateBook(bookUpdate);
        //update book info
        _bookService.Update(bookUpdate);
        // add new relations between other tables
        _seriesService.AddBook(SelectedSeries, bookUpdate);
        UpdateBookSelection();
        BookSelectionIsChanged.Invoke();
    }

    private bool SaveNewBookCommandCanExecute()
    {
        if (SelectedBook == null)
        {
            return false;
        }
        else
        {
            if (SelectedBook.Id == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void SaveNewBookExecuteCommand()
    {
        if (CheckIfIsbn13HasRightFormat(SelectedId))
        {
            var newBook = CollectDataAndFillBookModel();
            // Add a newBook to database 
            _bookService.Add(newBook);

            // Complete newBook by adding relation with series 
            if (SelectedSeries != null && SelectedSeries != _defaultSerie)
            {
                _seriesService.AddBook(SelectedSeries, newBook);
            }
            // Complete newBook by adding relations with authors

            if (SelectedAuthors != null)
            {
                _authorService.AddBook(newBook);
            }

            UpdateBookSelection();
            BookSelectionIsChanged.Invoke();
        }
        else
        {
            MessageBox.Show("Wrong ISBN13");
        }
    }

    private BookModel CollectDataAndFillBookModel()
    {
        SeriesModel tempSeries;
        if (SelectedSeries is null || SelectedSeries == _defaultSerie)
        {
            tempSeries = new SeriesModel();
        }
        else
        {
            tempSeries = SelectedSeries;
        }

        var existingAuthors = AuthorList;

        foreach (var author in AuthorList)
        {
            if (author.IsSelected == true && SelectedAuthors.All(a => a.Id != author.Id))
            {
                SelectedAuthors.Add(author);
            }
        }
        var bookModel = new BookModel()
        {
            Id = SelectedId,
            Title = SelectedTitle,
            Language = SelectedLanguage,
            Price = (decimal)SelectedPrice,
            Publisher = SelectedPublisher,
            Translator = SelectedTranslator,
            Published = DateOnly.FromDateTime(SelectedPublishDate),
            Series = new List<SeriesModel> { tempSeries },
            Authors = SelectedAuthors
        };

        return bookModel;
    }
    private void PopulateBookInfo()
    {
        CultureInfo culture = new CultureInfo("se-SE");
        DateTime now = DateTime.Now;

        if (SelectedBook != _defaultBook)
        {
            SelectedTitle = SelectedBook.Title;
            SelectedLanguage = SelectedBook.Language;
            SelectedPrice = (double)SelectedBook.Price;
            SelectedPublisher = SelectedBook.Publisher;
            SelectedTranslator = SelectedBook.Translator;
            SelectedId = SelectedBook.Id;
            SelectedPublishDate = SelectedBook.Published.ToDateTime(TimeOnly.MinValue);
            if (SeriesModelsList != null && SelectedBook != null && SelectedBook.Series.Count > 0)
            {
                SelectedSeriesIndex = SeriesModelsList.FindIndex(s => s.SeriesName == SelectedBook.Series[0].SeriesName);
            }
            else
            {
                SelectedSeriesIndex = -1;
            }
        }
        else
        {
            SelectedTitle = "";
            SelectedLanguage = "";
            SelectedPrice = 0;
            SelectedPublisher = "";
            SelectedTranslator = "";
            SelectedId = "";
            SelectedPublishDate = now;
            SelectedSeriesIndex = -1;
        }

        if (SelectedBook != _defaultBook)
        {
            IsReadOnlyIsbnTxtBox = true;
        }
        else
        {
            IsReadOnlyIsbnTxtBox = false;
        }
        UpdateAuthorsSelection();
    }
    private void UpdateAuthorsSelection() // if IsSelected is true then the checkbox by authors name will be checked (in View)
    {
        var temp = _authorService.GetAll();
        foreach (var author in SelectedBook.Authors)
        {
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == author.Id)
                {
                    temp[i].IsSelected = true;
                }
            }
        }

        AuthorList = temp;
        SelectedAuthors = temp.Where(b => b.IsSelected == true).ToList();

    }
    private void UpdateBookSelection()
    {
        var temp = _bookService.GetAll();
        temp.Add(_defaultBook);
        BookSelection = temp;
    }

    private bool CheckIfIsbn13HasRightFormat(string isbn13)
    {
        string regexPattern = @"^\d{13}$";
        Regex regex = new Regex(regexPattern);
        return regex.IsMatch(isbn13);
    }
}