using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Interfaces;
using Labb2_DbFirst_Template.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb2_DbFirst_Template.Repositories;

public class AuthorRepository : IRepository<Author,int>
{
    private readonly BookstoreDbErDiagramContext _context;

    public AuthorRepository()
    {
        _context = BookstoreDbErDiagramContext.GetInstance();
    }
    public Author GetById(int id)
    {
        return _context.Authors.Find(id);
    }

    public IEnumerable<Author> GetAll()
    {
        return _context.Authors;
    }

    public void Add(Author entity)
    {
        var highestId = _context.Authors.Select(a => a.Id).Max();
        var nextId = highestId + 1;
        entity.Id = nextId;
        _context.Authors.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Author entity)
    {
        var existingAuthor = _context.Authors.Find(entity.Id);

        existingAuthor.FirstName = entity.FirstName;
        existingAuthor.LastName = entity.LastName;
        existingAuthor.Born = entity.Born;
        existingAuthor.Died = entity.Died;
        existingAuthor.Isbn13s = entity.Isbn13s;
       _context.SaveChanges();
    }

    public void Delete(Author entity)
    {
        if (entity != null)
        {
            _context.Authors.Remove(entity);
            _context.SaveChanges();
        }
    }

}