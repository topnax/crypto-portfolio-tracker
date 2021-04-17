using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using SqlKata.Execution;

namespace Repository
{
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

        public void Update(T entry)
        {
            Db.Get().Query(_tableName).Where("id", _getEntryId(entry)).AsUpdate(ToRow(entry));
        }

        public void Delete(T entry)
        {
            Db.Get().Query(_tableName).Where("id", _getEntryId(entry)).AsDelete();
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
}