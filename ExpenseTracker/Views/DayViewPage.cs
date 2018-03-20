using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Common;
using ExpenseTracker.ContentViews;
using Xamarin.Forms;

namespace ExpenseTracker.Views
{
    public class DayViewPage : ContentPage
    {
        ListView listView;
        StackLayout stkDate, stkTotalView;
        Label lblNoExpense, lblDate, lblTotal;
        DatePicker dtpkr;
        ObservableCollection<Expense> observableCollection = new ObservableCollection<Expense>();
        ExpenseData data = new ExpenseData();

        public DayViewPage(DateTime dateTime)
        {
            try
            {
                #region Page
                Title = "Expense Tracker";

                var toolAdd = new ToolbarItem
                {
                    Text = "Add"
                };
                ToolbarItems.Add(toolAdd);
                #endregion

                #region Controls
                listView = new ListView
                {
                    HasUnevenRows = true,
                    SeparatorVisibility = SeparatorVisibility.None,
                    IsPullToRefreshEnabled = true,
                    ItemsSource = observableCollection
                };

                listView.ItemTemplate = new DataTemplate(() =>
                {
                    var lblDescription = new Label
                    {
                        TextColor = Colors.Black75,
                        FontSize = Styles.FontSmall
                    };
                    lblDescription.SetBinding(Label.TextProperty, "Description");
                    var lblDateTime = new Label
                    {
                        TextColor = Colors.Black50,
                        FontSize = Styles.FontSmall
                    };
                    lblDateTime.SetBinding(Label.TextProperty, "DateTime", stringFormat: "{0:hh:mm tt}");
                    var lblAmount = new Label
                    {
                        TextColor = Colors.Blue75,
                        FontSize = Styles.FontSmall,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    };
                    lblAmount.SetBinding(Label.TextProperty, "Amount", stringFormat: "{0:F2}");
                    var grid = new Grid
                    {
                        Padding = 16,
                        ColumnDefinitions =
                        {
                            new ColumnDefinition{ Width = new GridLength(3, GridUnitType.Star) },
                            new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Auto) },
                        },
                        RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                        }
                    };
                    grid.Children.Add(lblDescription);
                    grid.Children.Add(lblDateTime, 0, 1);
                    grid.Children.Add(lblAmount, 1, 0);
                    Grid.SetRowSpan(lblAmount, 2);

                    return new ViewCell
                    {
                        View = grid
                    };
                });

                dtpkr = new DatePicker
                {
                    Date = dateTime.Date,
                    IsVisible = false
                };

                lblDate = new Label
                {
                    Text = dtpkr.Date.ToLongDateString(),
                    TextColor = Colors.Black75,
                    HorizontalOptions = LayoutOptions.Center
                };

                stkDate = new StackLayout
                {
                    Padding = 16,
                    BackgroundColor = Colors.Black25,
                    Children =
                    {
                        lblDate
                    }
                };

                lblTotal = new Label
                {
                    Text = "0.00",
                    FontSize = Styles.FontMedium,
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.End
                };

                stkTotalView = new StackLayout
                {
                    Spacing = 0,
                    Padding = 16,
                    BackgroundColor = Colors.Primary,
                    Children =
                    {
                        new Label
                        {
                            Text = "Total:",
                            FontSize = Styles.FontSmall,
                            TextColor = Colors.WhiteSmoke,
                            HorizontalOptions = LayoutOptions.End
                        },
                        lblTotal
                    }
                };

                lblNoExpense = new Label
                {
                    Margin = new Thickness(0, 60, 0, 0),
                    Text = "No expenses found",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black25,
                    FontSize = Styles.FontMedium,
                    IsVisible = false
                };
                #endregion

                #region Container
                Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                    {
                        stkDate,
                        dtpkr,
                        lblNoExpense,
                        listView,
                        stkTotalView,
                        new AdControlContainer()
                    }
                };
                #endregion

                #region Gestures/Events
                toolAdd.Clicked += (sender, args) =>
                {
                    Navigation.PushAsync(new AddExpensePage(dtpkr.Date), true);
                };

                listView.RefreshCommand = new Command(() =>
                {
                    LoadData(dtpkr.Date);
                });

                listView.ItemTapped += async (sender, args) =>
                {
                    listView.SelectedItem = null;

                    var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");
                    if (action == "Edit")
                    {
                        await Navigation.PushAsync(new AddExpensePage(DateTime.Today, (Expense)args.Item), true);
                    }
                    else if (action == "Delete")
                    {
                        var deleteAction = await DisplayAlert("Warning", "Are you sure you want to delete this expense?", "Yes", "No");
                        if (deleteAction)
                        {
                            data.Delete<Expense>(((Expense)args.Item).Id);
                            LoadData(dtpkr.Date);
                        }
                    }
                };

                stkDate.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        dtpkr.Focus();
                    }),
                    NumberOfTapsRequired = 1
                });

                dtpkr.DateSelected += (sender, args) =>
                {
                    lblDate.Text = dtpkr.Date.ToLongDateString();

                    LoadData(dtpkr.Date);
                };
                #endregion
            }
            catch (Exception ex)
            {
                Utils.LogMessage("DayViewPage()", ex);
            }
        }

        private void LoadData(DateTime dateTime)
        {
            try
            {
                listView.IsRefreshing = true;
                observableCollection.Clear();
                var items = data.GetAllExpensesByDateTime(dateTime);
                foreach (var item in items)
                {
                    observableCollection.Add(item);
                }
                lblTotal.Text = String.Format("{0:0.00}", items.Sum(x => x.Amount));

                if (observableCollection.Count == 0)
                    lblNoExpense.IsVisible = true;
                else
                    lblNoExpense.IsVisible = false;
                listView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Utils.LogMessage("DayViewPage.LoadData()", ex);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                LoadData(dtpkr.Date);

                return false;
            });
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            listView.HeightRequest = height;
        }
    }
}