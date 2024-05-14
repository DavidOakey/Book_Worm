using BookWorm.Models.Enums;

namespace BookWorm.Models
{
	public static class Constants
	{
		private static readonly Dictionary<Genres, GenreData> _genreData = new Dictionary<Genres, GenreData>
		{
			{ Genres.Fantasy, new GenreData{Genre = Genres.Fantasy, Name = "Fantasy", Icon="fantasy.png"} },
			{ Genres.Historical, new GenreData{Genre = Genres.Historical, Name = "Historical", Icon="historical.png"} },
			{ Genres.Horror, new GenreData{Genre = Genres.Horror, Name = "Horror", Icon="horror.png"} },
			{ Genres.NonFiction, new GenreData{Genre = Genres.NonFiction, Name = "Non-Fiction", Icon="nonfiction.png"} },
			{ Genres.Mystery, new GenreData{Genre = Genres.Mystery, Name = "Mystery", Icon="mystery.png"} },
			{ Genres.Romance, new GenreData{Genre = Genres.Romance, Name = "Romance", Icon="romance.png"} },
			{ Genres.ScienceFiction, new GenreData{Genre = Genres.ScienceFiction, Name = "Science Fiction", Icon="scifi.png"} },
			{ Genres.Other, new GenreData{Genre = Genres.Other, Name = "Other", Icon="other.png"} }
		};

		public static Dictionary<Genres, GenreData> GenreInfo { get => _genreData; }

		public static string GetGenreText(Genres genre)
		{
			return GenreInfo?.GetValueOrDefault(genre)?.Name ?? "-";
		}

		internal static string GenreIcon(Genres genre)
		{
			return GenreInfo?.GetValueOrDefault(genre)?.Icon ?? "other.png";
		}
	}
}
