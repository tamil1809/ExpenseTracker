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
    public class SearchExpenseResultPage : ContentPage
    {
        ListView listView;
        StackLayout stkTotalView;
        Label lblNoExpense, lblTotal;
        ObservableCollection<Expense> observableCollection = new ObservableCollection<Expense>();
        ExpenseData data = new ExpenseData();
        string keyword;
        DateTime startDate, endDate;

        public SearchExpenseResultPage(string keyword, DateTime startDate, DateTime endDate)
        {
            try
            {
                #region Page
                Title = "Results";

                this.keyword = keyword;
                this.startDate = startDate;
                this.endDate = endDate;
                #endregion

                #region Controls
                listView = new ListView
                {
                    HasUnevenRows = true,
                    SeparatorVisibility = SeparatorVisibility.None,
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
                    lblDateTime.SetBinding(Label.TextProperty, "DateTime", stringFormat: "{0:D}");
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
                        lblNoExpense,
                        listView,
                        stkTotalView,
                        new AdControlContainer()
                    }
                };
                #endregion

                #region Gestures/Events
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
                            Search();
                        }
                    }
                };
                #endregion
            }
            catch (Exception ex)
            {
                Utils.LogMessage("SearchExpenseResultPage()", ex);
            }
        }

        private void Search()
        {
            try
            {
                observableCollection.Clear();
                var items = data.Search(keyword, startDate, endDate);
                foreach (var item in items)
                {
                    observableCollection.Add(item);
                }
                lblTotal.Text = String.Format("{0:0.00}", items.Sum(x => x.Amount));

                if (observableCollection.Count == 0)
                    lblNoExpense.IsVisible = true;
                else
                    lblNoExpense.IsVisible = false;
            }
            catch (Exception ex)
            {
                Utils.LogMessage("SearchExpenseResultPage.Search()", ex);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Search();
        }
    }
}