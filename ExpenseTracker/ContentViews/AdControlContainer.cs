using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ExpenseTracker.ContentViews
{
	public class AdControlContainer : ContentView
	{
		public AdControlContainer ()
		{
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };

            grid.Children.Add(new AdControlView());

            Content = grid;
		}
	}
}