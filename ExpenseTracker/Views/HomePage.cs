using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ExpenseTracker.Views
{
    public class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            Master = new SideMenuPage();

            Detail = new NavigationPage(new DayViewPage(DateTime.Today));

            MessagingCenter.Subscribe<SideMenuPage, string>(this, "MenuChanged", (sender, args) =>
            {
                NavigationPage navPage = null;
                if (args == "Day")
                {
                    navPage = new NavigationPage(new DayViewPage(DateTime.Today));
                }
                else if (args == "Month")
                {
                    navPage = new NavigationPage(new MonthViewPage());
                }
                else if (args == "Search")
                {
                    navPage = new NavigationPage(new SearchExpensePage());
                }
                else if (args == "Settings")
                {
                    navPage = new NavigationPage(new SettingsPage());
                }

                Detail = navPage;

                this.IsPresented = false;
            });
        }
    }
}