using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpenseTracker.iOS.Implementations;
using Foundation;
using UIKit;
using Xamarin.Forms;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Common;
using SQLite;
using System.IO;

[assembly: Dependency(typeof(DatabaseConnection_iOS))]
namespace ExpenseTracker.iOS.Implementations
{
    public class DatabaseConnection_iOS : IDatabaseConnection
    {
        public SQLiteConnection DatabaseConnection()
        {
            string libraryFolder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            var path = Path.Combine(libraryFolder, AppSettings.DatabaseName);
            return new SQLiteConnection(path);
        }

        public void BackupDatabase()
        {
            string libraryFolder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            var databaseFile = Path.Combine(libraryFolder, AppSettings.DatabaseName);
            string backupFile = Path.Combine(libraryFolder, AppSettings.DatabaseBackupName);
            var isBackupExist = File.Exists(backupFile);
            if (isBackupExist)
            {
                File.Delete(backupFile);
            }
            File.Copy(databaseFile, backupFile);
        }

        public void RestoreDatabase()
        {
            string libraryFolder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            var databaseFile = Path.Combine(libraryFolder, AppSettings.DatabaseName);
            string backupFile = Path.Combine(libraryFolder, AppSettings.DatabaseBackupName);
            var isFileExist = File.Exists(databaseFile);
            if (isFileExist)
            {
                File.Delete(databaseFile);
            }
            File.Copy(backupFile, databaseFile);
        }
    }
}