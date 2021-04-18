using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using SqlKata.Execution;

namespace Repository
{
    // Implements IRepository using a SqlKataDatabase
    public abstract class SqlKataRepository<T> : IRepository<T>
    {
        protected readonly SqlKataDatabase Db;
        private readonly string _tableName;

        public SqlKataRepository(SqlKataDatabase db, string tableName)
        {
            this.Db = db;
            this._tableName = tableName;
        }

        public abstract object ToRow(T entry);

        public abstract T FromRow(dynamic d);

        protected abstract int _getEntryId(T entry);

        public int Add(T entry)
        {
            var id = Db.Get().Query(_tableName).InsertGetId<int>(ToRow(entry));
            return id;
        }

        public bool Update(T entry)
        {
            var updated = Db.Get().Query(_tableName).Where("id", _getEntryId(entry)).Update(ToRow(entry));
            return updated > 0;
        }

        public bool Delete(T entry)
        {
            Db.Get().Query(_tableName).Where("id", _getEntryId(entry)).AsDelete();
            // TODO
            return true;
        }

        public T Get(int id)
        {
            var result = Db.Get().Query(_tableName).Where("id", id).First();
            if (result == null)
            {
                return default;
            }

            return FromRow(result);
        }

        public List<T> All()
        {
            List<T> items = new List<T>();
            foreach (var row in Db.Get().Query(_tableName).Select().Get())
            {
                items.Add(FromRow(row));
            }

            return items;
        }
    }

    public class SqlKataRepositoryException : Exception
    {
        public SqlKataRepositoryException(string message) : base(message)
        {
        }
    }
}