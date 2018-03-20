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
    public class SearchExpensePage : ContentPage
    {
        Entry txtKeyword;
        DatePicker dtpkrStart, dtpkrEnd;

        public SearchExpensePage()
        {
            try
            {
                #region Page
                Title = "Expense Tracker";

                var toolSearch = new ToolbarItem
                {
                    Text = "Search"
                };
                ToolbarItems.Add(toolSearch);
                #endregion

                #region Controls
                txtKeyword = new Entry
                {
                    Keyboard = Keyboard.Chat
                };

                dtpkrStart = new DatePicker
                {
                    Date = DateTime.Today.Date.AddDays(-10),
                    Format = "D"
                };

                dtpkrEnd = new DatePicker
                {
                    Date = DateTime.Today.Date,
                    Format = "D"
                };
                #endregion

                #region Container
                Content = new StackLayout
                {
                    Spacing = 10,
                    Padding = 16,
                    Children =
                    {
                        new Label
                        {
                            Text = "Keyword",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        txtKeyword,
                        new Label
                        {
                            Margin = new Thickness(0, 16, 0, 0),
                            Text = "Start Date",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        dtpkrStart,
                        new Label
                        {
                            Margin = new Thickness(0, 16, 0, 0),
                            Text = "End Date",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        dtpkrEnd,
                        new AdControlContainer()
                    }
                };
                #endregion

                #region Gestures/Events
                toolSearch.Clicked += (sender, args) =>
                {
                    if (dtpkrStart.Date > dtpkrEnd.Date)
                    {
                        DisplayAlert("Warning", "Start date must be before end date.", "Ok");
                        return;
                    }

                    Navigation.PushAsync(new SearchExpenseResultPage(string.IsNullOrEmpty(txtKeyword.Text) ? string.Empty : txtKeyword.Text, dtpkrStart.Date, dtpkrEnd.Date), true);
                };
                #endregion
            }
            catch (Exception ex)
            {
                Utils.LogMessage("SearchExpensePage()", ex);
            }
        }
    }
}