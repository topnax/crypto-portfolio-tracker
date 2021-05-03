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
        protected readonly string tableName;

        public SqlKataRepository(SqlKataDatabase db, string tableName)
        {
            this.Db = db;
            this.tableName = tableName;
        }

        public abstract object ToRow(T entry);

        public abstract T FromRow(dynamic d);

        protected abstract int _getEntryId(T entry);

        public int Add(T entry)
        {
            var id = Db.Get().Query(tableName).InsertGetId<int>(ToRow(entry));
            return id;
        }

        public bool Update(T entry)
        {
            var updated = Db.Get().Query(tableName).Where("id", _getEntryId(entry)).Update(ToRow(entry));
            return updated > 0;
        }

        public bool Delete(T entry)
        {
            Db.Get().Query(tableName).Where("id", _getEntryId(entry)).Delete();
            return true;
        }

        public T Get(int id)
        {
            var result = Db.Get().Query(tableName).Where("id", id).FirstOrDefault();
            if (result == null)
            {
                return default;
            }

            return FromRow(result);
        }

        public List<T> GetAll() => RowsToObjects(Db.Get().Query(tableName).Select().Get());

        protected List<T> RowsToObjects(IEnumerable<dynamic> rows)
        {
            List<T> items = new List<T>();
            foreach (var row in rows)
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