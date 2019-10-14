using System;
using System.Collections.Generic;
using System.Text;

namespace HydraPlot2
{
    public enum DatabaseErrorTypes
    {
        PK_VIOLATION,
        NOT_DEFINED,
        DATA_INVALID
    }

    public class DatabaseException : Exception
    {


        private DatabaseErrorTypes databaseError;

        public DatabaseErrorTypes DatabaseError
        {
            get { return databaseError; }
        }

        public DatabaseException(string message, DatabaseErrorTypes errType)
            : base(message)
        {
            databaseError = errType;
        }


    }
}
