using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atown10_CMobile.Services;
using System.IO;
using Xamarin.Forms;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(Atown10_CMobile.Droid.DatabaseConnection))]

namespace Atown10_CMobile.Droid
{
    class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteAsyncConnection DbConnection()
        {
            var dbName = "Database.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            return new SQLiteAsyncConnection(path);
        }
    }
}