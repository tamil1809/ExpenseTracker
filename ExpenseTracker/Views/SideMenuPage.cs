using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpenseTracker.Common;
using Xamarin.Forms;

namespace ExpenseTracker.Views
{
	public class SideMenuPage : ContentPage
	{
		public SideMenuPage ()
		{
            Title = "Menu";
            BackgroundColor = Colors.Primary;

            var stkDay = new StackLayout
            {
                Spacing = 16,
                Padding = 16,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Image
                    {
                        Source = "ic_md_day_view.png",
                        Aspect = Aspect.AspectFit
                    },
                    new Label
                    {
                        Text = "Day",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall
                    }
                }
            };

            var stkMonth = new StackLayout
            {
                Spacing = 16,
                Padding = 16,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Image
                    {
                        Source = "ic_md_month_view.png",
                        Aspect = Aspect.AspectFit
                    },
                    new Label
                    {
                        Text = "Month",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall
                    }
                }
            };

            var stkSearch = new StackLayout
            {
                Spacing = 16,
                Padding = 16,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Image
                    {
                        Source = "ic_md_search.png",
                        Aspect = Aspect.AspectFit
                    },
                    new Label
                    {
                        Text = "Search",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall
                    }
                }
            };

            var stkSettings = new StackLayout
            {
                Spacing = 16,
                Padding = 16,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Image
                    {
                        Source = "ic_md_settings.png",
                        Aspect = Aspect.AspectFit
                    },
                    new Label
                    {
                        Text = "Settings",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall
                    }
                }
            };

            var stkVersion = new StackLayout
            {
                Spacing = 0,
                Margin = 16,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Children = 
                {
                    new Label
                    {
                        Text = "v1.0",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall
                    },
                    new Label
                    {
                        Text = "Expense Tracker",
                        TextColor = Colors.White,
                        FontSize = Styles.FontMedium,
                        FontAttributes = FontAttributes.Bold
                    }
                }
            };

            Content = new StackLayout
            {
                Spacing = 0,
                Margin = new Thickness(0, 20, 0, 0),
                Children =
                {
                    new Label
                    {
                        Margin = 16,
                        Text = "View",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall - 2
                    },
                    stkDay,
                    stkMonth,
                    new Label
                    {
                        Margin = 16,
                        Text = "Extra",
                        TextColor = Colors.White,
                        FontSize = Styles.FontSmall - 2
                    },
                    stkSearch,
                    stkSettings,
                    stkVersion
                }
            };

            stkDay.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    MessagingCenter.Send<SideMenuPage, string>(this, "MenuChanged", "Day");
                }),
                NumberOfTapsRequired = 1
            });

            stkMonth.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    MessagingCenter.Send<SideMenuPage, string>(this, "MenuChanged", "Month");
                }),
                NumberOfTapsRequired = 1
            });

            stkSearch.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    MessagingCenter.Send<SideMenuPage, string>(this, "MenuChanged", "Search");
                }),
                NumberOfTapsRequired = 1
            });

            stkSettings.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    MessagingCenter.Send<SideMenuPage, string>(this, "MenuChanged", "Settings");
                }),
                NumberOfTapsRequired = 1
            });
        }
	}
}