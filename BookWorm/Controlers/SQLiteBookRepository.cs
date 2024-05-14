using BookWorm.Interfaces;
using BookWorm.Models;
using BookWorm.Models.SQLite;
using SQLite;

namespace BookWorm.Controlers
{
	public class SQLiteBookRepository : IBookRepository
	{
		private const string _dbFilename = "BookWorm.db3";

		private const SQLiteOpenFlags _flags =
			SQLiteOpenFlags.ReadWrite |
			SQLiteOpenFlags.Create |
			SQLiteOpenFlags.SharedCache;

		private static string DatabasePath =>
			Path.Combine(FileSystem.AppDataDirectory, _dbFilename);

		private SQLiteAsyncConnection? Database;
		public SQLiteBookRepository()
		{
			Init();
		}

		private async void Init()
		{
			try
			{
				if (Database != null)
				{
					return;
				}

				Database = new SQLiteAsyncConnection(DatabasePath, _flags);
				await Database.CreateTableAsync<BookDB>();
			}
			catch (Exception ex)
			{
				int i = 9;
			}
		}

		public void DeleteAllData()
		{
			try
			{
				Database?.DeleteAllAsync<BookDB>();
			}
			catch (Exception ex)
			{

			}
		}

		public void DeleteBook(Book book)
		{
			try
			{
				DeleteBook(book.BookId);
			}
			catch (Exception ex)
			{

			}
		}

		public void DeleteBook(Guid bookId)
		{
			try
			{
				Database.DeleteAsync<BookDB>(bookId);
			}
			catch (Exception ex)
			{

			}
		}

		public async Task<IEnumerable<Book>> GetAllBooks()
		{
			try
			{
				if (Database != null)
				{
					// There is currently an issue with calling ToListAsync on an empty table(it never returns)
					// ConfigureAwait(false) seems to be a solution.
					// Found https://github.com/praeclarum/sqlite-net/issues/809
					var bookTable = await Database.Table<BookDB>().ToListAsync().ConfigureAwait(false);
					return bookTable.Select(b => new Book(b)) ?? new List<Book>();
				}
			}
			catch (Exception ex)
			{
				int i = 9;
			}
			return new List<Book>();
		}

		public async Task<Book?> GetBookById(Guid bookId)
		{
			try
			{
				return new Book(await Database.GetAsync<BookDB>(bookId));
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public async void UpdateOrAddBook(Book book)
		{
			try
			{
				await Database.InsertOrReplaceAsync(new BookDB(book));
			}
			catch (Exception ex)
			{

			}
		}
	}
}
