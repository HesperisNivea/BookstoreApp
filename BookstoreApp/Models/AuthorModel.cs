using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labb2_DbFirst_Template.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookstoreApp.Models;

public class AuthorModel : INotifyPropertyChanged
{
    private bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName
    {
        get { return FirstName + " " + LastName;}
    }
    
    public DateOnly Born { get; set; }

    public DateOnly? Died { get; set; }

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}