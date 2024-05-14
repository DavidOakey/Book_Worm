using BookWorm.Controlers;
using BookWorm.Interfaces;
using BookWorm.Models;
using BookWorm.Models.Enums;
using BookWorm.ViewModels.Cells;
using System.Windows.Input;

namespace BookWorm.ViewModels
{
	public class BookListViewModel : BaseViewModel
	{
		public ICommand SettingsCommand => new Command(SettingsClick);
		public ICommand SearchCommand => new Command(SearchClick);
		public ICommand AddBookCommand => new Command(AddBookClick);
		public ICommand BackGroundCommand => new Command(BackGroundClick);
		public ICommand ViewBookCommand => new Command(BookMenuShowBookClicked);
		public ICommand DeleteCommand => new Command(BookMenuDeleteBookClicked);
		public ICommand DeleteAllBooksCommand => new Command(DeleteAllBooksClicked);
		public ICommand AboutCommand => new Command(AboutClicked);

		public ICommand ViewEditBookBackGroundCommand => new Command(ViewEditBookBackGroundClick);

		private ViewEditBookViewViewModel? _currentBookData;
		private Book? _selectedBook { get; set; }
		private List<Book>? _bookList;
		private List<BookListCellViewModel>? _activeBookList;
		private bool _showEditVisible = false;
		private bool _bookMenuVisible = false;
		private bool _settingsMenuVisible = false;
		private bool _searchMenuVisible = false;
		private string _searchText = string.Empty;

		IBookRepository _bookRepo = new SQLiteBookRepository();
		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				if (_searchText != value)
				{
					_searchText = value;
					SetFilteredList();
					OnPropertyChanged();
				}
			}
		}

		public ViewEditBookViewViewModel? BookSelected
		{
			get
			{
				return null;
			}
			set
			{
				if (value != null)
				{
					ShowAddEditView(value.BookData, ViewMode.View);
				}
				OnPropertyChanged();
			}
		}

		public ViewEditBookViewViewModel? CurrentBookData
		{
			get
			{
				return _currentBookData;
			}
			set
			{
				_currentBookData = value;
				OnPropertyChanged();
			}
		}

		public bool BookMenuVisible
		{
			get
			{
				return _bookMenuVisible;
			}
			set
			{
				_bookMenuVisible = value;
				OnPropertyChanged();
			}
		}

		public bool SettingsMenuVisible
		{
			get
			{
				return _settingsMenuVisible;
			}
			set
			{
				_settingsMenuVisible = value;
				OnPropertyChanged();
			}
		}

		public bool SearchMenuVisible
		{
			get
			{
				return _searchMenuVisible;
			}
			set
			{
				_searchMenuVisible = value;
				OnPropertyChanged();
			}
		}

		public List<Book> BookList
		{
			get
			{
				return _bookList ?? new List<Book>();
			}
			set
			{
				_bookList = value;
				SetFilteredList();
				OnPropertyChanged();
			}
		}
		public List<BookListCellViewModel> ActiveBookList
		{
			get
			{
				return _activeBookList ?? new List<BookListCellViewModel>();
			}
			set
			{
				_activeBookList = value;
				OnPropertyChanged();
			}
		}

		private async void ReloadBookList()
		{
			BookList = (await _bookRepo.GetAllBooks()).ToList();
		}

		private void SetFilteredList()
		{
			int index = 0;
			_activeBookList = _bookList?.Where(b =>
					b.Title.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
					b.Author.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
					b.Description.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
					b.Published.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase))
				.OrderBy(b=>b.Title)
				.Select(b => new BookListCellViewModel
				{
					IndexPosition = index++,
					BookData = b,
					OnView = new Action<Book?>(b => ViewBook(b)),
					OnMore = new Action<Book?>(b => ShowMenu(b))
				}).ToList() ?? new List<BookListCellViewModel>();
			OnPropertyChanged(nameof(ActiveBookList));
		}

		private void ViewBook(Book? book)
		{
			if (book != null)
			{
				ShowAddEditView(book, ViewMode.View);
			}
		}

		private void ShowMenu(Book? book)
		{
			if (book != null)
			{
				_selectedBook = book;
				BookMenuVisible = true;
			}
		}

		private void SettingsClick()
		{
			SettingsMenuVisible = true;
		}

		private void SearchClick()
		{
			SearchMenuVisible = true;
		}

		private void AddBookClick()
		{
			ShowAddEditView(new Book
			{
				BookId = Guid.NewGuid()
			}, ViewMode.Add);
		}

		private void ShowAddEditView(Book book, ViewMode viewMode)
		{
			CurrentBookData = new ViewEditBookViewViewModel
			{
				BookData = book,
				CurrentViewMode = viewMode,
				OnClose = () => ShowEditVisible = false,
				OnSave = UpdateCurrentBook
			};
			ShowEditVisible = true;
		}

		public bool ShowEditVisible
		{
			get
			{
				return _showEditVisible;
			}
			set
			{
				if (_showEditVisible != value)
				{
					_showEditVisible = value;
					OnPropertyChanged();
				}
			}
		}
		private void UpdateCurrentBook(Book book)
		{
			if (CurrentBookData != null)
			{
				SaveBook(book);
				ActiveBookList?.Where(b=>b.BookData?.BookId == book.BookId).ToList().ForEach(b => b.Refresh());
			}
		}
		private void SaveBook(Book book)
		{

			_bookRepo.UpdateOrAddBook(book);
			ShowEditVisible = false;
			ReloadBookList();
		}

		private void BookMenuShowBookClicked()
		{
			if (_selectedBook != null)
			{
				BookMenuVisible = false;
				ShowAddEditView(_selectedBook, ViewMode.View);
			}
		}

		private async void DeleteAllBooksClicked()
		{
			if (await Application.Current?.MainPage?.DisplayAlert("WARNING", "ALL BOOK DATA WILL BE LOST!\nCOINFIRM WIPE? ", "Yes", "No"))
			{
				_bookRepo?.DeleteAllData();
			}
			BookMenuVisible = false;
			ReloadBookList();
		}

		private async void BookMenuDeleteBookClicked()
		{
			if (_selectedBook != null &&
				await Application.Current.MainPage.DisplayAlert("Are you shore?", "Delete, " + (_selectedBook?.Title ?? "-"), "Yes", "No"))
			{
				_bookRepo?.DeleteBook(_selectedBook);
			}
			BookMenuVisible = false;
			ReloadBookList();
		}

		private async void AboutClicked()
		{
			SettingsMenuVisible = false;
			await Application.Current.MainPage.DisplayAlert("About", "This is a test app to demonstrate various coding techniques using Xamarin Maui", "Ok");	
		}

		private void ViewEditBookBackGroundClick()
		{
			if (CurrentBookData?.CurrentViewMode == ViewMode.View)
			{
				ShowEditVisible = false;
			}
		}

		private void BackGroundClick()
		{
			BookMenuVisible = false;
			SettingsMenuVisible = false;
		}
	}
}
