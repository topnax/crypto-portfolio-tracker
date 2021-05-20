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
        // hold a reference to the SqlKataDatabase instance
        protected readonly SqlKataDatabase Db;
        
        // name of the table this repository utilizes
        protected readonly string TableName;

        /// <param name="db">Instance of the database object</param>
        /// <param name="tableName">Name of the table to be utilized</param>
        public SqlKataRepository(SqlKataDatabase db, string tableName)
        {
            Db = db;
            TableName = tableName;
        }

        /// <summary>
        /// Converts a object of type T to a generic untyped object (to a table row)
        /// </summary>
        /// <param name="entry">An object of type T to be converted</param>
        /// <returns>Generic object (table row) representing the given object of type T</returns>
        protected abstract object ToRow(T entry);

        /// <summary>
        /// Converts a generic object (table row) to an object of type T
        /// </summary>
        /// <param name="d">Object to be converted to an object of type T</param>
        /// <returns>Returns an object of type T converted from the generic object (table row)</returns>
        protected abstract T FromRow(dynamic d);

        /// <summary>
        /// Returns an ID of the given object of type T
        /// </summary>
        /// <param name="entry">Object whose ID should be returner</param>
        /// <returns>ID of the given object</returns>
        protected abstract int GetEntryId(T entry);

        // use the InsertGetId method of the SqlKata library in order to insert a new row and get it's ID
        public int Add(T entry) => Db.Get().Query(TableName).InsertGetId<int>(ToRow(entry));

        // updates the given object of type T using the WHERE and UPDATE method calls, returns a boolean flag indicating
        // whether at least one table row was updated
        public bool Update(T entry) =>
            Db.Get().Query(TableName).Where(SqlSchema.TableIdPrimaryKey, GetEntryId(entry)).Update(ToRow(entry)) > 0;

        // deletes the given object of type T and returns a boolean flag indicating whether at least one table row was deleted
        public bool Delete(T entry) => Db.Get().Query(TableName).Where(SqlSchema.TableIdPrimaryKey, GetEntryId(entry)).Delete() > 0;
        
        public T Get(int id)
        {
            // find a table rows based on the given ID
            var result = Db.Get().Query(TableName).Where(SqlSchema.TableIdPrimaryKey, id).FirstOrDefault();
            
            // convert the given table row a ta an object of type T
            return result == null ? default : FromRow(result);
        }

        // select all rows from the database and converts them to objects of type T
        public List<T> GetAll() => RowsToObjects(Db.Get().Query(TableName).Select().Get());

        // converts a collection of table rows to a list of objects of type T
        protected List<T> RowsToObjects(IEnumerable<dynamic> rows) => rows.Select(row => (T) FromRow(row)).ToList();
        
    }
}