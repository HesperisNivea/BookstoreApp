using BookstoreApp.Models;
using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;

namespace BookstoreApp.Services;

public class StockService
{
    private readonly StockRepository _stockRepository = new();

    public List<StockModel> GetAll()
    {
        return _stockRepository.GetAllStocks().Select(
            stock => new StockModel()
            {
                ShopId = stock.ShopId,
                Isbn13 = stock.Isbn13,
                Amount = stock.Amount,
                ShopName = stock.Shop.ShopName,
                BookTitle = stock.Book.Title
            }).ToList();
    }

    public StockModel GetById((int, string) id)
    {
        var temp = _stockRepository.GetById(id);
        if (temp is null)
        {
            return null;
        }
        else
        {
            return new StockModel()
            {
                ShopId = temp.ShopId,
                Isbn13 = temp.Isbn13,
                Amount = temp.Amount,
                ShopName = temp.Shop.ShopName,
                BookTitle = temp.Book.Title
            };
        }

    }
    private void RemoveStock(StockModel stockModel)
    {
        _stockRepository.Delete(_stockRepository.GetById((stockModel.ShopId, stockModel.Isbn13)));
    }

    public void RemoveOneBook(StockModel stockModel)
    {
        _stockRepository.AmountDecrease(_stockRepository.GetById((stockModel.ShopId, stockModel.Isbn13)));
    }

    public void AddStock(StockModel stockModel)
    {
        _stockRepository.Add(new Stock()
        {
            Amount = stockModel.Amount,
            Isbn13 = stockModel.Isbn13,
            ShopId = stockModel.ShopId
        });
    }

    public void AddOneBook(StockModel stockModel)
    {
        if (stockModel == null)
        {
            AddStock(stockModel);
            return;
        }
        _stockRepository.AmountIncrease(_stockRepository.GetById((stockModel.ShopId, stockModel.Isbn13)));
    }
}