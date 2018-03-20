using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Common
{
    public class Utils
    {
        public static void LogMessage(string method, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(">>>>>>>>>>" + "-Method: " + method + " -Message: " + ex.Message + " -Stack Trace: " + ex.StackTrace);
        }
    }
}