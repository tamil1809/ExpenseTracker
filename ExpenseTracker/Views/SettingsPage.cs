using System;
using ExpenseTracker.Common;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Data;
using Xamarin.Forms;

namespace ExpenseTracker.Views
{
    public class SettingsPage : ContentPage
    {
        Label lblLastBackup;
        Button btnBackupNow, btnRestoreBackup;
        public SettingsPage()
        {
            #region Page
            Title = "Settings";
            #endregion

            #region Controls
            lblLastBackup = new Label
            {
                Text = "",
                TextColor = Colors.Black75,
                FontSize = Styles.FontMedium
            };
            btnBackupNow = new Button
            {
                Text = "Back Up Now",
                TextColor = Colors.Blue75,
                BackgroundColor = Color.Transparent,
                BorderWidth = 0,
                Margin = new Thickness(0, 32, 0, 0)
            };
            btnRestoreBackup = new Button
            {
                Text = "Restore Back Up",
                TextColor = Colors.Blue75,
                BackgroundColor = Color.Transparent,
                BorderWidth = 0,
                Margin = new Thickness(0, 32, 0, 0)
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
                        Text = "Last Backup:",
                        TextColor = Colors.Black25,
                        FontSize = Styles.FontSmall
                    },
                    lblLastBackup,
                    btnBackupNow,
                    btnRestoreBackup
                }
            };
            #endregion

            #region Gestures/Events
            btnBackupNow.Clicked += (sender, args) => 
            {
                if (Application.Current.Properties.ContainsKey("LastBackupDateTime"))
                {
                    Application.Current.Properties["LastBackupDateTime"] = DateTime.Now;
                }
                else
                {
                    Application.Current.Properties.Add("LastBackupDateTime", DateTime.Now);
                }

                LoadLastBackupDateTime();

                DependencyService.Get<IDatabaseConnection>().BackupDatabase();

                DisplayAlert("Success", "Back up created successfully.", "Ok");

                BaseData.CloseConnection();
            };
            btnRestoreBackup.Clicked += (sender, args) =>
            {
                DependencyService.Get<IDatabaseConnection>().RestoreDatabase();

                DisplayAlert("Success", "Back up restored successfully.", "Ok");

                BaseData.CloseConnection();
            };
            #endregion

            LoadLastBackupDateTime();
        }

        private void LoadLastBackupDateTime()
        {
            if (Application.Current.Properties.ContainsKey("LastBackupDateTime"))
            {
                var lastBackupDateTime = (DateTime)Application.Current.Properties["LastBackupDateTime"];
                lblLastBackup.Text = lastBackupDateTime.ToString("MMMMM dd, yyyy hh:mm:ss tt");
                btnRestoreBackup.IsVisible = true;
            }
            else
            {
                lblLastBackup.Text = "No backups found";
                btnRestoreBackup.IsVisible = false;
            }
        }
    }
}