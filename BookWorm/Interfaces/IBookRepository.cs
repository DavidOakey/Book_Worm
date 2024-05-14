using BookWorm.Models;

namespace BookWorm.Interfaces
{
	public interface IBookRepository
	{
		Task<IEnumerable<Book>> GetAllBooks();
		void UpdateOrAddBook(Book book);
		void DeleteBook(Book book);
		void DeleteBook(Guid bookId);
		Task<Book?> GetBookById(Guid bookId);
		void DeleteAllData();
	}
}
