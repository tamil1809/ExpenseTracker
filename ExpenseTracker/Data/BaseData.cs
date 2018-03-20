using ExpenseTracker.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data
{
    public abstract class BaseData
    {
        internal static SQLiteConnection database = null;

        public BaseData()
        {
            try
            {
                if (database == null)
                {
                    database = DependencyService.Get<IDatabaseConnection>().DatabaseConnection();

                    CreateTables();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateTables()
        {
            try
            {
                database.CreateTable<Expense>(CreateFlags.AllImplicit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Save(BaseModel obj)
        {
            if (obj.Id == 0)
                database.Insert(obj);
            else
                database.Update(obj);
        }

        public T Load<T>(long id) where T : new()
        {
            return database.Get<T>(id);
        }

        public int Delete<T>(long id) where T : new()
        {
            return database.Delete<T>(id);
        }

        public int DeleteAll<T>(long id) where T : new()
        {
            return database.DeleteAll<T>();
        }

        public static void CloseConnection()
        {
            database?.Close();

            database = null;
        }
    }
}