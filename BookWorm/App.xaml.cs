using BookWorm.Controlers;
using BookWorm.Interfaces;
using BookWorm.ViewModels;
using BookWorm.Views;

namespace BookWorm
{
	public partial class App : Application
	{
		public App()
		{
			try
			{
				InitializeComponent();

				IBookRepository bookRepo = new SQLiteBookRepository();
				MainPage = new BookList { BindingContext = new BookListViewModel { BookList = bookRepo.GetAllBooks().Result.ToList() } };
			}
			catch 
			{
				// Error logging can be added here
			}
		}
	}
}
