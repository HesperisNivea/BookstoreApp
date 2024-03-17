using System.ComponentModel.DataAnnotations;
using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Interfaces;
using Labb2_DbFirst_Template.Models;

namespace Labb2_DbFirst_Template.Repositories;

public class ShopRepository : IRepository<Shop,int>
{
    private readonly BookstoreDbErDiagramContext _context = BookstoreDbErDiagramContext.GetInstance();

    public Shop GetById(int id)
    {
        return _context.Shops.Find(id);
    }

    public IEnumerable<Shop> GetAll()
    {
        return _context.Shops;
    }

    public void Add(Shop entity) 
    {
        _context.Shops.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Shop entity) 
    {
        _context.Shops.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Shop entity) 
    {
        _context.Shops.Remove(entity);
        _context.SaveChanges();
    }

}