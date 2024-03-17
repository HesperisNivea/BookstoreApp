using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using BookstoreApp.Models;
using BookstoreApp.Services;
using BookstoreApp.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookstoreApp.ViewModel;

public class AuthorViewModel : ObservableObject
{
    
    private static AuthorService _authorService = new AuthorService();

    public List<AuthorModel> _authorModelsList = _authorService.GetAll();

    private AuthorModel _selectedAuthor;

    public AuthorModel? SelectedAuthor
    {
        get { return _selectedAuthor; }
        set
        {
            _selectedAuthor = value;

            if (_selectedAuthor != null) //&& _selectedAuthor != _defaultAuthor
            {
                PopulateAuthorInfo();
            }
            UpdateExistingAuthor.NotifyCanExecuteChanged();
            SaveNewAuthor.NotifyCanExecuteChanged();
            RemoveAuthor.NotifyCanExecuteChanged();
            OnPropertyChanged();
        }
    }

    private bool _isDeceased;
    
    public bool IsDeceased
    {
        get { return _isDeceased; }
        set
        {
            _isDeceased = value; 
            OnPropertyChanged();
        }
    }


    public List<AuthorModel> AuthorModelsList
    {
        get { return _authorModelsList; }
        set
        {
            _authorModelsList = value;
            OnPropertyChanged();
        }
    }

    private string _selectedFirstName;

	public string SelectedFirstName 
	{
		get { return _selectedFirstName; }
        set
        {
            _selectedFirstName = value;
            OnPropertyChanged();
        }
	}

	private string _selectedLastName;

	public string SelectedLastName
	{
		get { return _selectedLastName; }
        set
        {
            _selectedLastName = value;
            OnPropertyChanged();
        }
	}

	private DateTime _selectedDateOfBirth;

	public DateTime SelectedDateOfBirth
    {
		get { return _selectedDateOfBirth; }
        set
        {
            _selectedDateOfBirth = value;
            OnPropertyChanged();
        }
	}

	private DateTime _selectedDateOfDeath ;

	public DateTime SelectedDateOfDeath
    {
		get { return _selectedDateOfDeath; }
        set
        {
            _selectedDateOfDeath = value;
            OnPropertyChanged();
        }
	}

	public IRelayCommand SaveNewAuthor { get; }
	public IRelayCommand RemoveAuthor { get; }
	public IRelayCommand UpdateExistingAuthor { get; }
    public IRelayCommand FillInTextBoxWithInfoFromSelectedAuthor { get; }

    public static Action AuthorSelectionIsChanged;

    private readonly AuthorModel _defaultAuthor = new AuthorModel() { FirstName = "--NEW", LastName = "AUTHOR--" };
    public AuthorViewModel()
    {
        UpdateAuthorsSelection();
        SaveNewAuthor = new RelayCommand(SaveNewAuthorExecuteCommand, SaveNewAuthorCommandCanExecute);
        RemoveAuthor = new RelayCommand(RemoveAuthorExecuteCommand, RemoveAuthorCommandCanExecute);
        UpdateExistingAuthor = new RelayCommand(UpdateExistingAuthorExecuteCommand, UpdateExistingAuthorCommandCanExecute);
        FillInTextBoxWithInfoFromSelectedAuthor = new RelayCommand(FillInTextBoxWithInfoFromSelectedAuthorExecuteCommand, FillInTextBoxWithInfoFromSelectedAuthorCommandCanExecute);
    }

    private bool FillInTextBoxWithInfoFromSelectedAuthorCommandCanExecute()
    {
        if (SelectedAuthor.Id == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    private void FillInTextBoxWithInfoFromSelectedAuthorExecuteCommand()
    {
        PopulateAuthorInfo();
    }

    private void PopulateAuthorInfo()
    {
        CultureInfo culture = new CultureInfo("se-SE");
        DateTime now = DateTime.Now;
        SelectedFirstName = SelectedAuthor.FirstName;
        SelectedLastName = SelectedAuthor.LastName;
        SelectedDateOfBirth = SelectedAuthor.Born.ToDateTime(TimeOnly.MinValue);
        if (SelectedAuthor.Died != null)
        {
            IsDeceased = true;
            SelectedDateOfDeath = SelectedAuthor.Died.Value.ToDateTime(TimeOnly.MinValue);
        }
        else
        {
            IsDeceased = false;
            SelectedDateOfDeath = now;
        }

        if (SelectedAuthor == _defaultAuthor)
        {
            IsDeceased = false;
            SelectedFirstName = string.Empty;
            SelectedLastName = string.Empty;
            SelectedDateOfBirth = now;
            SelectedDateOfDeath = now;
        }
    }


    private bool UpdateExistingAuthorCommandCanExecute()
    {
      

        if (SelectedAuthor == null)
        {
            return false;
        }
        else
        {
            if (SelectedAuthor.Id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private void UpdateExistingAuthorExecuteCommand()
    {
        _authorService.Update(CollectDataAndFillAuthorModel());
        UpdateAuthorsSelection();
        AuthorSelectionIsChanged.Invoke();
    }
    private bool RemoveAuthorCommandCanExecute()
    {
        if (SelectedAuthor == null || SelectedAuthor.Id == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void RemoveAuthorExecuteCommand()
    {
        _authorService.Remove(SelectedAuthor);
        UpdateAuthorsSelection();
        AuthorSelectionIsChanged.Invoke();
    }


    private bool SaveNewAuthorCommandCanExecute()
    {
        if (SelectedAuthor == null)
        {
            return false;
        }
        else
        {
            if (SelectedAuthor.Id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void SaveNewAuthorExecuteCommand()
    {
        _authorService.Add(CollectDataAndFillAuthorModel());
        UpdateAuthorsSelection();
        AuthorSelectionIsChanged.Invoke();
    }

    public AuthorModel CollectDataAndFillAuthorModel()
    {
        var authorModel = new AuthorModel()
        {
            Id = SelectedAuthor.Id,
            FirstName = SelectedFirstName,
            LastName = SelectedLastName,
            Born = DateOnly.FromDateTime(SelectedDateOfBirth),
        };

        if (IsDeceased == true)
        {
            authorModel.Died = DateOnly.FromDateTime(SelectedDateOfDeath);
        }
        else
        {
            authorModel.Died = null;
        }

        return authorModel;
    }

    private void UpdateAuthorsSelection()
    {
        var temp = _authorService.GetAll();
        temp.Add(_defaultAuthor);
        AuthorModelsList = temp;
    }

}
