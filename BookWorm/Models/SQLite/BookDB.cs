using BookWorm.Models.Enums;
using SQLite;

namespace BookWorm.Models.SQLite
{
	public class BookDB
    {
		[PrimaryKey]
		public Guid BookId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public Genres Genre { get; set; } = Genres.Other;
		public string Published { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public BookDB()
		{

		}
		public BookDB(Book book)
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
