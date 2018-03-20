using ExpenseTracker.Droid.Implementations;
using ExpenseTracker.Common;
using Xamarin.Forms;
using SQLite;
using System.IO;

[assembly: Dependency(typeof(DatabaseConnection_Droid))]
namespace ExpenseTracker.Droid.Implementations
{
    public class DatabaseConnection_Droid : Interfaces.IDatabaseConnection
    {
        public SQLiteConnection DatabaseConnection()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), AppSettings.DatabaseName);
            return new SQLiteConnection(path);
        }

        public void BackupDatabase()
        {
            var databaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), AppSettings.DatabaseName);
            string backupFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), AppSettings.DatabaseBackupName);
            var isBackupExist = File.Exists(backupFile);
            if (isBackupExist)
            {
                File.Delete(backupFile);
            }
            File.Copy(databaseFile, backupFile);
        }

        public void RestoreDatabase()
        {
            var databaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), AppSettings.DatabaseName);
            string backupFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), AppSettings.DatabaseBackupName);
            var isFileExist = File.Exists(databaseFile);
            if (isFileExist)
            {
                File.Delete(databaseFile);
            }
            File.Copy(backupFile, databaseFile);
        }
    }
}