using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Interfaces;
using Labb2_DbFirst_Template.Models;

namespace Labb2_DbFirst_Template.Repositories;

public class SeriesRepository : IRepository<Series,int>
{
    private readonly BookstoreDbErDiagramContext _context = BookstoreDbErDiagramContext.GetInstance();
    public Series GetById(int id)
    {
        return _context.Series.FirstOrDefault(s => s.Id == id);
    }

    public IEnumerable<Series> GetAll()
    {
        return _context.Series;
    }

    public void Add(Series entity)
    {
        _context.Series.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Series entity)
    {
        _context.Series.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Series entity)
    {
        _context.Series.Remove(entity);
        _context.SaveChanges();
    }

}