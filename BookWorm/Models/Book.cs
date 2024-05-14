using BookWorm.Models.Enums;
using BookWorm.Models.SQLite;

namespace BookWorm.Models
{
	public class Book
	{
		public Guid BookId { get;  set; }
		public string Title { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public Genres Genre { get; set; } = Genres.Other;
		public string Published { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public Book()
		{

		}
		public Book(BookDB book)
		{
			BookId = book.BookId;
			Title = book.Title;
			Author = book.Author;
			Genre = book.Genre;
			Published = book.Published;
			Description = book.Description;
		}
	}
}
