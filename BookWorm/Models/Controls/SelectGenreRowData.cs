using BookWorm.Models.Enums;
using BookWorm.ViewModels;

namespace BookWorm.Models.Controls
{
	public class SelectGenreRowData : BaseViewModel
    {
        public View? RowView { get; set; }
        public Image? SelectedIcon { get; set; }
		public string? Icon { get; set; }
        public string? Name { get; set; }
        public Genres Genre { get; set; }
	}
}
