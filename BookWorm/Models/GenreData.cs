using BookWorm.Models.Enums;

namespace BookWorm.Models
{
	public class GenreData
	{
		public Genres Genre { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Icon { get; set; } = string.Empty;
	}
}
