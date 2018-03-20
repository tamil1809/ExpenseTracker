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
    public class MonthViewPage : ContentPage
    {
        ListView listView;
        StackLayout stkMonth, stkTotalView;
        Picker pickerMonth, pickerYear;
        Label lblNoExpense, lblMonth, lblTotal;
        ObservableCollection<DayExpense> observableCollection = new ObservableCollection<DayExpense>();
        ExpenseData data = new ExpenseData();

        private class DayExpense
        {
            public DateTime DateTime { get; set; }
            public string Date { get; set; }
            public double Amount { get; set; }
        }

        public MonthViewPage()
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
                    var lblDate = new Label
                    {
                        TextColor = Colors.Black75,
                        FontSize = Styles.FontSmall
                    };
                    lblDate.SetBinding(Label.TextProperty, "Date");
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
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                        }
                    };
                    grid.Children.Add(lblDate);
                    grid.Children.Add(lblAmount, 1, 0);

                    return new ViewCell
                    {
                        View = grid
                    };
                });

                pickerMonth = new Picker
                {
                    IsVisible = false
                };

                pickerYear = new Picker
                {
                    IsVisible = false
                };

                pickerMonth.Items.Add("January");
                pickerMonth.Items.Add("February");
                pickerMonth.Items.Add("March");
                pickerMonth.Items.Add("April");
                pickerMonth.Items.Add("May");
                pickerMonth.Items.Add("June");
                pickerMonth.Items.Add("July");
                pickerMonth.Items.Add("August");
                pickerMonth.Items.Add("September");
                pickerMonth.Items.Add("October");
                pickerMonth.Items.Add("November");
                pickerMonth.Items.Add("December");

                var startYear = DateTime.Today.Year - 10;
                for (int i = startYear; i <= DateTime.Today.Year; i++)
                {
                    pickerYear.Items.Add(i.ToString());
                }

                lblMonth = new Label
                {
                    Text = "",
                    TextColor = Colors.Black75,
                    HorizontalOptions = LayoutOptions.Center
                };

                stkMonth = new StackLayout
                {
                    Padding = 16,
                    BackgroundColor = Colors.Black25,
                    Children =
                    {
                        lblMonth
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
                        pickerMonth, pickerYear,
                        stkMonth,
                        lblNoExpense,
                        listView,
                        stkTotalView,
                        new AdControlContainer()
                    }
                };
                #endregion

                #region Load Before Gestures
                pickerMonth.SelectedItem = DateTime.Today.ToString("MMMM");
                pickerYear.SelectedItem = DateTime.Today.Year.ToString();
                lblMonth.Text = pickerMonth.SelectedItem.ToString() + ", " + pickerYear.SelectedItem.ToString();
                #endregion

                #region Gestures/Events
                toolAdd.Clicked += (sender, args) =>
                {
                    Navigation.PushAsync(new AddExpensePage(DateTime.Today.Date), true);
                };

                listView.RefreshCommand = new Command(() =>
                {
                    LoadData();
                });

                listView.ItemTapped += async (sender, args) =>
                {
                    listView.SelectedItem = null;

                    var action = await DisplayActionSheet("Action", "Cancel", null, "View", "Delete");
                    if (action == "View")
                    {
                        await Navigation.PushAsync(new DayViewPage(((DayExpense)args.Item).DateTime.Date), true);
                    }
                    else if (action == "Delete")
                    {
                        var deleteAction = await DisplayAlert("Warning", "Are you sure you want to delete all the expenses for this day?", "Yes", "No");
                        if (deleteAction)
                        {
                            data.DeleteByDay(((DayExpense)args.Item).DateTime);
                            LoadData();
                        }
                    }
                };

                stkMonth.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        pickerMonth.Focus();
                    }),
                    NumberOfTapsRequired = 1
                });

                pickerMonth.SelectedIndexChanged += (sender, args) =>
                {
                    lblMonth.Text = pickerMonth.SelectedItem.ToString();
                    lblMonth.Text += ", " + pickerYear.SelectedItem.ToString();
                    pickerYear.Focus();

                    LoadData();
                };

                pickerYear.SelectedIndexChanged += (sender, args) =>
                {
                    lblMonth.Text = pickerMonth.SelectedItem.ToString();
                    lblMonth.Text += ", " + pickerYear.SelectedItem.ToString();

                    LoadData();
                };
                #endregion
            }
            catch (Exception ex)
            {
                Utils.LogMessage("MonthViewPage()", ex);
            }
        }

        public void LoadData()
        {
            try
            {
                listView.IsRefreshing = true;
                observableCollection.Clear();
                var items = data.GetAllExpensesByMonthAndYear(pickerMonth.SelectedIndex + 1, Convert.ToInt32(pickerYear.SelectedItem));
                var newIitems = items.GroupBy(x => x.DateTime.Date);
                foreach (var item in newIitems)
                {
                    observableCollection.Add(new DayExpense
                    {
                        Date = item.Key.ToLongDateString(),
                        Amount = item.Sum(x => x.Amount),
                        DateTime = item.Key
                    });
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
                Utils.LogMessage("MonthViewPage.LoadData()", ex);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                LoadData();

                return false;
            });
        }
    }
}