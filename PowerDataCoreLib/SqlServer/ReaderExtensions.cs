using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace PowerDataCoreLib.SqlServer
{
    public static class ReaderExtensions
    {
        public static T? ReadNullableValue<T>(this SqlDataReader reader, string columnName) where T : struct
        {
            var result = reader[columnName];

            if (result is T)
                return (T)result;

            if (result == DBNull.Value)
                return null;

            throw new Exception($"Cannot convert column '{columnName}' value of type '{result.GetType().FullName}' to type of '{typeof(T).FullName}'");
        }

        public static T ReadValue<T>(this SqlDataReader reader, string columnName) where T : struct
        {
            var result = reader[columnName];

            if (result is T)
                return (T)result;

            throw new Exception($"Cannot convert column '{columnName}' value of type '{result.GetType().FullName}' to type of '{typeof(T).FullName}'");
        }

        public static string ReadStringOrNull(this SqlDataReader reader, string columnName)
        {
            var result = reader[columnName];

            if (result is string)
                return (string)result;

            if (result == DBNull.Value)
                return null;

            throw new Exception($"Cannot convert column '{columnName}' value of type '{result.GetType().FullName}' to type of '{typeof(string).FullName}'");
        }
    }
}
