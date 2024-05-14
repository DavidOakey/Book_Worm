using BookWorm.Models;
using BookWorm.Models.Controls;
using BookWorm.Models.Enums;

namespace BookWorm.Views.Controls;

public class SelectGenre : ContentView
{
	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SelectGenre), 14.0, propertyChanged: UpdateControl);
	public static readonly BindableProperty BackgroundUnselectedColorProperty = BindableProperty.Create(nameof(BackgroundUnselectedColor), typeof(Color), typeof(SelectGenre), Colors.White, propertyChanged: UpdateControl);
	public static readonly BindableProperty BackgroundSelectedColorProperty = BindableProperty.Create(nameof(BackgroundSelectedColor), typeof(Color), typeof(SelectGenre), Colors.Gray, propertyChanged: UpdateControl);
	public static readonly BindableProperty SelectedGenreProperty = BindableProperty.Create(nameof(SelectedGenre), typeof(Genres?), typeof(SelectGenre), null, defaultBindingMode:BindingMode.TwoWay);
	public static readonly BindableProperty OnCloseProperty = BindableProperty.Create(nameof(OnClose), typeof(Action), typeof(SelectGenre), null, propertyChanged: UpdateControl);

	private double _cornerRadius = 5;
	private Grid _mainGrid;
	private List<SelectGenreRowData> _selectedGenreRows = new List<SelectGenreRowData>();

	public Action OnClose
	{
		get => (Action)GetValue(OnCloseProperty);
		set => SetValue(OnCloseProperty, value);
	}

	public double FontSize
	{
		get => (double)GetValue(FontSizeProperty);
		set => SetValue(FontSizeProperty, value);
	}

	public Color BackgroundUnselectedColor
	{
		get => (Color)GetValue(BackgroundUnselectedColorProperty);
		set => SetValue(BackgroundUnselectedColorProperty, value);
	}

	public Color BackgroundSelectedColor
	{
		get => (Color)GetValue(BackgroundSelectedColorProperty);
		set => SetValue(BackgroundSelectedColorProperty, value);
	}

	public Genres? SelectedGenre
	{
		get => (Genres?)GetValue(SelectedGenreProperty);
		set => SetValue(SelectedGenreProperty, value);
	}

	private static void UpdateControl(BindableObject bindable, object oldValue, object newValue)
	{
		SelectGenre selectGenre = (SelectGenre)bindable;
		selectGenre?.CreateList();
	}

	public SelectGenre()
	{
		Grid _screenGrid;
		BackgroundColor = Color.FromArgb("#BB000000");
		Content = _screenGrid = new Grid
		{
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
				new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
				new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
			},
			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
			},
			ColumnSpacing = 0,
			RowSpacing = 0
		};

		_screenGrid.Add(new Frame
		{
			Margin = new Thickness(5, 10, 5, 10),
			BackgroundColor = Colors.White,
			CornerRadius = 2,
			Content = _mainGrid = new Grid
			{
				RowDefinitions = new RowDefinitionCollection(),
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
				},
				ColumnSpacing = 0,
				RowSpacing = 0
			}
		}, 1, 1);

		GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(() =>
			{
				OnClose?.Invoke();
			})
		});

		CreateList();
	}

	private void CreateList()
	{
		_mainGrid.Children.Clear();
		_mainGrid.RowDefinitions.Clear();
		_selectedGenreRows.Clear();

		SelectGenreRowData row;
		Constants.GenreInfo.Values.ToList().ForEach(g =>
		{
			_mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
			_selectedGenreRows.Add(row = CreateRow(g));
			_mainGrid.Add(row.RowView, 0, _mainGrid.RowDefinitions.Count - 1);
		});
	}

	private SelectGenreRowData CreateRow(GenreData genreData)
	{
		SelectGenreRowData row = new SelectGenreRowData
		{
			Genre = genreData.Genre,
			Icon = genreData.Icon,
			Name = genreData.Name
		};

		row.RowView = new HorizontalStackLayout
		{
			Spacing = 0,
			HeightRequest = 50,
			BackgroundColor = genreData.Genre == SelectedGenre ? BackgroundSelectedColor : BackgroundUnselectedColor,
			Children = {
				new Image
				{
					Source = genreData.Icon,
					Margin = new Thickness(5,5,5,5),
					Aspect = Aspect.AspectFit
				},
				new Label
				{
					FontSize = 12,
					Text = genreData.Name,
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Start,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
				}
			}
		};

		(row.RowView as HorizontalStackLayout)?.Add(row.SelectedIcon = new Image
		{
			Source = genreData.Genre == SelectedGenre ? "tick.png" : "blank.png",
			Margin = new Thickness(5, 5, 5, 5),
			Aspect = Aspect.AspectFit
		});

		row.RowView.GestureRecognizers.Add(new TapGestureRecognizer 
		{ 
			Command = new Command(()=>
			{
				SelectedGenre = genreData.Genre;
				UpdateRows();
				OnClose?.Invoke();
			}) 
		});

		return row;
	}

	private void UpdateRows()
	{
		_selectedGenreRows?.ForEach(r => {
			r.SelectedIcon.Source = r.Genre == SelectedGenre ? "tick.png" : "blank.png";
			r.RowView.BackgroundColor = r.Genre == SelectedGenre ? BackgroundSelectedColor : BackgroundUnselectedColor;
		});
	}
}