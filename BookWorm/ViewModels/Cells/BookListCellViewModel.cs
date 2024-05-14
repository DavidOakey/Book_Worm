using BookWorm.Models;
using BookWorm.Models.Enums;
using System.Windows.Input;

namespace BookWorm.ViewModels.Cells
{
	public class BookListCellViewModel : BaseViewModel
	{
		private int _indexPosition;
		public Action<Book?>? OnView;
		public Action<Book?>? OnMore;

		public ICommand ViewCommand => new Command(ViewClick);
		public ICommand MoreCommand => new Command(MoreClick);
		
		public Book? BookData { get; set; }

		public Color CellBackground
		{
			get
			{
				return IndexPosition % 2 == 0 ? Colors.Gray : Colors.LightGray;
			}
		}

		public int IndexPosition
		{
			get
			{
				return _indexPosition;
			}
			set
			{
				_indexPosition = value;
			}
		}

		public string Title
		{
			get
			{
				return BookData?.Title ?? "-";
			}
	}
		public string Author
		{
			get
			{
				return BookData?.Author ?? "-";
			}
		}
		public string Genre
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
				return BookData?.Published ?? "-";
			}
		}
		public string GenreIcon
		{
			get
			{
				return Constants.GenreIcon(BookData?.Genre ?? Genres.Other);
			}
		}

		public void Refresh()
		{
			OnPropertyChanged(nameof(Title));
			OnPropertyChanged(nameof(Author));
			OnPropertyChanged(nameof(GenreIcon));
			OnPropertyChanged(nameof(Published));
		}

		public void MoreClick()
		{
			OnMore?.Invoke(BookData);
		}

		public void ViewClick()
		{
			OnView?.Invoke(BookData);
		}
	}
}
