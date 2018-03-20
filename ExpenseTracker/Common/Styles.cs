using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ExpenseTracker.Common
{
    class Styles
    {
        public static double FontLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        public static double FontMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        public static double FontSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        public static double FontMicro = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
        public static double FontDefault = Device.GetNamedSize(NamedSize.Default, typeof(Label));

        public static double IconSize = 24;
        public static double PrimaryTextSize = 16;
        public static double SecondaryTextSize = 14;
    }
}