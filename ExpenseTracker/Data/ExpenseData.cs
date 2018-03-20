using System;
using System.Collections.Generic;
using System.Text;
using ExpenseTracker.Models;
using ExpenseTracker.Common;

namespace ExpenseTracker.Data
{
    class ExpenseData : BaseData
    {
        public IEnumerable<Expense> GetAllExpensesByDateTime(DateTime dateTime)
        {
            try
            {
                var startDate = dateTime;
                var endDate = dateTime.AddDays(1).AddMinutes(-1);
                return database.Table<Expense>().Where(x => x.DateTime >= startDate && x.DateTime <= endDate).OrderByDescending(x => x.DateTime);
            }
            catch (Exception ex)
            {
                Utils.LogMessage("ExpenseData.GetAllExpensesByDateTime()", ex);
            }
            return null;
        }

        public IEnumerable<Expense> GetAllExpensesByMonthAndYear(int month, int year)
        {
            try
            {
                var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                var startDate = new DateTime(year, month, 1);
                var endDate = new DateTime(year, month, calendar.GetDaysInMonth(year, month));
                return database.Table<Expense>().Where(x => x.DateTime >= startDate && x.DateTime <= endDate).OrderBy(x => x.DateTime);
            }
            catch (Exception ex)
            {
                Utils.LogMessage("ExpenseData.GetAllExpensesByMonthAndYear()", ex);
            }
            return null;
        }

        public int DeleteByDay(DateTime dateTime)
        {
            try
            {
                var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                var startDate = dateTime.Date;
                var endDate = dateTime.Date.AddDays(1).AddMinutes(-1);
                return database.Table<Expense>().Delete(x => x.DateTime >= startDate && x.DateTime <= endDate);
            }
            catch (Exception ex)
            {
                Utils.LogMessage("ExpenseData.DeleteByDay()", ex);
            }
            return 0;
        }

        public IEnumerable<Expense> Search(string keyword, DateTime startDate, DateTime endDate)
        {
            try
            {
                keyword = keyword.ToLower();
                endDate = endDate.AddDays(1).AddSeconds(-1);
                if (string.IsNullOrEmpty(keyword))
                {
                    return database.Table<Expense>().Where(x => x.Description == "--NO DESCRIPTION--" || (x.DateTime >= startDate && x.DateTime <= endDate)).OrderBy(x => x.DateTime);
                }
                else
                {
                    return database.Table<Expense>().Where(x => x.Description.ToLower().StartsWith(keyword) && x.DateTime >= startDate && x.DateTime <= endDate).OrderBy(x => x.DateTime);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage("ExpenseData.Search()", ex);
            }
            return null;
        }
    }
}
