using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Interfaces
{
    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DatabaseConnection();

        void BackupDatabase();

        void RestoreDatabase();
    }
}