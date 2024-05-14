using BookWorm.Models;
using BookWorm.Models.Enums;
using System.Windows.Input;

namespace BookWorm.ViewModels
{
	public class ViewEditBookViewViewModel : BaseViewModel
	{
		public ICommand LockCommand => new Command(LockClick);
		public ICommand SaveCommand => new Command(SaveClick);
		public ICommand CloseCommand => new Command(CloseClick);
		public ICommand GenreCommand => new Command(GenreClick);

		private ViewMode _currentViewMode = ViewMode.View;
		private Book? _bookData;
		private bool _selectGenreVisible = false;

		public Action? OnClose;
		public Action<Book>? OnSave;

		public bool LockedIconVisible
		{
			get
			{
				return CurrentViewMode != ViewMode.Add;
			}
		}

		public ViewMode CurrentViewMode
		{
			get
			{
				return _currentViewMode;
			}
			set
			{
				if (_currentViewMode != value)
				{
					_currentViewMode = value;
					OnPropertyChanged(nameof(InEditMode));
					OnPropertyChanged(nameof(LockedIconVisible));
					OnPropertyChanged(nameof(InViewMode));
					OnPropertyChanged(nameof(LockedIcon));
				}
			}
		}

		public bool SelectGenreVisible
		{
			get
			{
				return _selectGenreVisible;
			}
			set
			{
				if(_selectGenreVisible != value) 
				{ 
					_selectGenreVisible = value;
					OnPropertyChanged();
				}
			}
		}

		public bool InEditMode
		{
			get
			{
				return CurrentViewMode != ViewMode.View;
			}
		}

		public bool InViewMode
		{
			get
			{
				return CurrentViewMode == ViewMode.View;
			}
		}

		public string LockedIcon
		{
			get
			{
				switch(CurrentViewMode)
				{
					case ViewMode.View:
						return "lock.png";
					case ViewMode.Edit:
						return "unlock.png";
					default:
						return  "add.png";
				}
			}
		}

		public Book BookData
		{
			get
			{
				return _bookData;
			}
			set
			{
				if (_bookData != value)
				{
					_bookData = value;
				}
			}
		}
		public Guid BookId { get; set; }
		public string Title
		{
			get 
			{ 
				return (!InEditMode && string.IsNullOrWhiteSpace(BookData.Title)) ? "-" : BookData.Title; 
			}
			set 
			{
				if (BookData.Title != value)
				{
					BookData.Title = value;
					OnPropertyChanged();
				}
			}
		}

		public string Author
		{
			get
			{
				return (!InEditMode && string.IsNullOrWhiteSpace(BookData.Author)) ? "-" : BookData.Author;
			}
			set
			{
				if (BookData.Author != value)
				{
					BookData.Author = value;
					OnPropertyChanged();
				}
			}
		}

		public Genres Genre
		{
			get
			{
				return BookData.Genre;
			}
			set
			{
				if(BookData.Genre != value)
				{
					BookData.Genre = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(GenreText));
				}
			}
		}

		public string GenreText
		{
			get
			{				
				return Constants.GetGenreText(BookData?.Genre ?? Genres.Other);
			}			
		}

		public string Published
		{
			get
			{
				return (!InEditMode && string.IsNullOrWhiteSpace(BookData.Published)) ? "-" : BookData.Published;
			}
			set
			{
				if (BookData.Published != value)
				{
					BookData.Published = value;
					OnPropertyChanged();
				}
			}
		}

		public string Description
		{
			get
			{
				return (!InEditMode && string.IsNullOrWhiteSpace(BookData.Description)) ? "-" : BookData.Description;
			}
			set
			{
				if (BookData.Description != value)
				{
					BookData.Description = value;
					OnPropertyChanged();
				}
			}
		}

		private void LockClick()
		{
			CurrentViewMode = CurrentViewMode == ViewMode.Edit ? ViewMode.View: ViewMode.Edit;
		}

		private void CloseClick() 
		{
			OnClose?.Invoke();
		}
		private async void SaveClick()
		{
			if (await ValidateBookData())
			{
				OnSave?.Invoke(BookData);
			}
		}

		private void GenreClick()
		{
			if (InEditMode)
			{
				SelectGenreVisible = true;
			}
		}

		public Action OnSelectGenresClose
		{
			get
			{
				return new Action(() => SelectGenreVisible = false);
			}
		}

		private async Task<bool> ValidateBookData()
		{
			if(string.IsNullOrWhiteSpace(BookData.Title))
			{
				await Application.Current.MainPage.DisplayAlert("Warning", "Please enter a title!", "OK");
				return false;
			}

			if(string.IsNullOrEmpty(BookData.Author))
			{
				await Application.Current.MainPage.DisplayAlert("Warning", "Please enter a author!", "OK");
				return false ;
			}

			int publishYear;
			if (!int.TryParse(BookData.Published, out publishYear) || publishYear < 1001)
			{
				await Application.Current.MainPage.DisplayAlert("Warning", "Ensure Published date is of format yyyy", "OK");
				return false;
			}

			if(publishYear > DateTime.Now.Year)
			{
				return await Application.Current.MainPage.DisplayAlert("Warning", "Published date is in the future!\nContinue?", "Yes", "No");
			}
			return true;
		}
	}
}
