using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Interfaces;
using Labb2_DbFirst_Template.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb2_DbFirst_Template.Repositories;

public class StockRepository : IRepository<Stock, (int, string)>
{
    private readonly BookstoreDbErDiagramContext _context = BookstoreDbErDiagramContext.GetInstance();

    public List<Stock> GetAllStocks()
    {
        return _context.Stocks.Include(s => s.Book).Include(S => S.Shop).ToList();
    }
    public Stock GetById((int, string) id)
    {
        return _context.Stocks.FirstOrDefault(s => s.ShopId == id.Item1 && s.Isbn13 == id.Item2);
    }

    public IEnumerable<Stock> GetAll() 
    {
        return _context.Stocks.Include(s => s.Book).Include(S => S.Shop).ToList();
    }

    public void Add(Stock entity)
    {
        _context.Stocks.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Stock entity)
    {
        _context.Stocks.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Stock entity)
    {
        _context.Stocks.Remove(entity);
        _context.SaveChanges();
    }

    public void AmountDecrease(Stock entity)
    {
        if(entity.Amount <= 1)
        {
            Delete(entity);
            return;
        }
        entity.Amount--;
        _context.SaveChanges();
    }

    public void AmountIncrease(Stock entity)
    {
        if (entity.Amount is null)
        {
            return;
        }
        entity.Amount++;
        _context.SaveChanges();
    }

}