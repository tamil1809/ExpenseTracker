using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Xamarin.Forms;

namespace ExpenseTracker.Views
{
    public class AddExpensePage : ContentPage
    {
        Entry txtAmount, txtDescription;
        DatePicker dtpkr;
        TimePicker tmpkr;

        public AddExpensePage(DateTime dateTime, Expense expense = null)
        {
            try
            {
                #region Page
                Title = "Add Expense";

                var toolSave = new ToolbarItem
                {
                    Text = "Save"
                };
                ToolbarItems.Add(toolSave);
                #endregion

                #region Controls
                txtAmount = new Entry
                {
                    Keyboard = Keyboard.Numeric
                };

                txtDescription = new Entry
                {
                    Keyboard = Keyboard.Chat
                };

                dtpkr = new DatePicker
                {
                    Date = dateTime.Date,
                    Format = "D"
                };
                tmpkr = new TimePicker
                {
                    Time = DateTime.Now.TimeOfDay
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
                            Text = "Amount",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        txtAmount,
                        new Label
                        {
                            Margin = new Thickness(0, 16, 0, 0),
                            Text = "Description",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        txtDescription,
                        new Label
                        {
                            Margin = new Thickness(0, 16, 0, 0),
                            Text = "Date",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        dtpkr,
                        new Label
                        {
                            Margin = new Thickness(0, 16, 0, 0),
                            Text = "Time",
                            TextColor = Colors.Black25,
                            FontSize = Styles.FontSmall
                        },
                        tmpkr
                    }
                };
                #endregion

                #region Gestures/Events
                txtAmount.Focus();

                txtAmount.Completed += (sender, args) =>
                {
                    txtDescription.Focus();
                };

                txtDescription.Completed += (sender, args) =>
                {
                    dtpkr.Focus();
                };

                toolSave.Clicked += (sender, args) =>
                {
                    try
                    {

                        var amount = txtAmount.Text;
                        double dAmount = 0.00;
                        var description = txtDescription.Text;

                        if (!string.IsNullOrEmpty(amount))
                        {
                            bool isValidAmount = Double.TryParse(amount, out dAmount);

                            if (isValidAmount && dAmount <= 0)
                                isValidAmount = false;

                            if (!isValidAmount)
                            {
                                DisplayAlert("Warning", "You must enter valid amount.", "Ok");
                                return;
                            }
                        }
                        else
                        {
                            DisplayAlert("Warning", "You must enter amount.", "Ok");
                            return;
                        }

                        if (string.IsNullOrEmpty(description))
                        {
                            description = "--NO DESCRIPTION--";
                        }

                        var data = new ExpenseData();
                        if (expense == null)
                        {
                            expense = new Expense();
                        }

                        expense.Amount = dAmount;
                        expense.Description = description;
                        expense.DateTime = dtpkr.Date + tmpkr.Time;

                        data.Save(expense);

                        Navigation.PopAsync(true);
                    }
                    catch (Exception ex)
                    {
                        Utils.LogMessage("AddExpensePage.Save()", ex);
                    }
                };
                #endregion

                //Load data if Edit
                if (expense != null)
                {
                    txtAmount.Text = expense.Amount.ToString();
                    if (expense.Description == "--NO DESCRIPTION--")
                        txtDescription.Text = string.Empty;
                    else
                        txtDescription.Text = expense.Description;
                    dtpkr.Date = expense.DateTime.Date;
                    tmpkr.Time = expense.DateTime.TimeOfDay;
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage("AddExpensePage()", ex);
            }
        }
    }
}