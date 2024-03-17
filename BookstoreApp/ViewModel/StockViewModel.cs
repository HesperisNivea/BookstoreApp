using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using BookstoreApp.Models;
using BookstoreApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.ViewModel;

public class StockViewModel : ObservableObject
{
    private static ShopService _shopService = new ShopService();

    private static BookService _bookService = new BookService();

    private static StockService _stockService = new StockService();

    #region Properties

    private List<StockModel>? _stocks;

    public List<StockModel>? Stocks
    {
        get { return _stocks; }
        set
        {
            _stocks = value;
            OnPropertyChanged();
        }
    }

    private List<BookModel> _books = _bookService.GetAll();

    public List<BookModel> Books
    {
        get { return _books; }
        set
        {
            _books = value;
            OnPropertyChanged();
        }
    }

    private BookModel? _selectedBook;

    public BookModel? SelectedBook
    {
        get { return _selectedBook; }
        set
        {
            Debug.WriteLine(value);
            _selectedBook = value;
            OnPropertyChanged();
            AddOneBookToStock.NotifyCanExecuteChanged();
            RemoveOneBookFromStock.NotifyCanExecuteChanged();
        }
    } 

    private List<ShopModel> _shop = _shopService.GetAll();

    public List<ShopModel> Shops
    {
        get { return _shop; }
        set { _shop = value; }
    }

    private ShopModel? _selectedShop;
    public ShopModel? SelectedShop
    {
        get { return _selectedShop; }
        set
        {
            _selectedShop = value;
            OnPropertyChanged();
            ShowStockContentPerStore.NotifyCanExecuteChanged();
            ShowStockContentPerStoreExecuteCommand();

        }
    }
    #endregion

    public IRelayCommand ShowStockContentPerStore { get; }
    public IRelayCommand AddOneBookToStock { get; }
    public IRelayCommand RemoveOneBookFromStock { get; }
    public StockViewModel() //?? 
    {
        BookViewModel.BookSelectionIsChanged += BookSelectionIsChanged;
        ShowStockContentPerStore = new RelayCommand(ShowStockContentPerStoreExecuteCommand, ShowStockContentPerStoreCommandCanExecute);
        AddOneBookToStock = new RelayCommand(AddOneBookToStockExecuteCommand, AddOneBookToStockCommandCanExecute);
        RemoveOneBookFromStock = new RelayCommand(RemoveOneBookFromStockExecuteCommand, RemoveOneBookFromStockCommandCanExecute);
    }

    private void BookSelectionIsChanged()
    {
        Books = _bookService.GetAll();
    }

    private bool RemoveOneBookFromStockCommandCanExecute() 
    {
         return SelectedBook is not null && SelectedShop is not null;
    }

    private void RemoveOneBookFromStockExecuteCommand() 
    {
        if (_stockService.GetById((SelectedShop.Id, SelectedBook.Id)) is null) 
        {
            MessageBox.Show("No match!!!");
            return;
        }
        var temp = _stockService.GetById((SelectedShop.Id, SelectedBook.Id));
        _stockService.RemoveOneBook(temp);

        UpdateStock();
        
    }

    private bool AddOneBookToStockCommandCanExecute()
    {
        return SelectedBook is not null && SelectedShop is not  null;
    }

    private void AddOneBookToStockExecuteCommand()
    {
        bool stockItemExists = Stocks.Exists(s => s.ShopId == SelectedShop.Id && s.Isbn13 == SelectedBook.Id);
        if (!stockItemExists) 
        {
            var newStock = new StockModel() { Isbn13 = SelectedBook.Id, ShopId = SelectedShop.Id, Amount = 1 };
            _stockService.AddStock(newStock);
            UpdateStock();
        }
        else
        {
            var temp = _stockService.GetById((SelectedShop.Id, SelectedBook.Id));
            _stockService.AddOneBook(temp);
            // UpdateStock
            UpdateStock();
        }
            
       

    }

    private bool ShowStockContentPerStoreCommandCanExecute()
    {
        return !string.IsNullOrEmpty(SelectedShop.ShopName);
    }

    private void ShowStockContentPerStoreExecuteCommand()
    {
        if (Stocks is not null)
        {
            Stocks.Clear();
        }

        Stocks = _stockService.GetAll().Where(s => s.ShopName == SelectedShop.ShopName)
            .ToList();
    }

    private void UpdateStock()
    {
        if (SelectedShop is null)
        {
            return;
        }

        if (Stocks is not null)
        {
            Stocks.Clear();
        }

        Stocks = _stockService.GetAll().Where(s => s.ShopName == SelectedShop.ShopName)
            .ToList();
    }
}