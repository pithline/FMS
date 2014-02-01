﻿using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.BusinessLogic.Helpers
{
    public sealed class SqliteHelper
    {
        public SqliteHelper()
        {


        }

        private static readonly SqliteHelper instance = new SqliteHelper();

        public static SqliteHelper Storage
        {
            get { return instance; }

        }

        private SQLite.SQLiteAsyncConnection connection;

        public SQLite.SQLiteAsyncConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public async void ConnectionDatabaseAsync()
        {
            if (connection == null)
            {
                var db = await ApplicationData.Current.RoamingFolder.GetFileAsync("SQLiteDB\\eqstramobility.sqlite");
                connection = new SQLite.SQLiteAsyncConnection(db.Path);
            }
        }

        public async void DropTable<T>() where T : new()
        {

            await this.Connection.DropTableAsync<T>();
        }

        public Task<List<T>> LoadTableAsync<T>()
            where T : ValidatableBindableBase,new()
            
        {
            return this.Connection.Table<T>().ToListAsync();
        }

        public Task<T> GetSingleRecordAsync<T>(Expression<Func<T,bool>> criteria)
            where T:ValidatableBindableBase,new()
        {
           return this.connection.Table<T>().Where(criteria).FirstOrDefaultAsync();
        }

        public async void InsertAllAsync<T>(IEnumerable<T> items) where T : new()
        {
            await this.Connection.InsertAllAsync(items);
        }


    }
}