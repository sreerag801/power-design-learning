using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PowerDataNet.SqlServer
{
    public interface ISqlReader
    {
        List<T> SqlExecuteReader<T>(string query, Action<SqlCommand> sqlCommand, Func<SqlDataReader, T> map);
        List<T> SqlExecuteReader<T>(string query, Func<SqlDataReader, T> map);
        T SqlExecuteObjectReader<T>(string query, Func<SqlDataReader, T> map) where T : class, new();
        int SqlExecuteNonQuery(string query, Action<SqlCommand> sqlCommand = null);
        int? SqlExecuteScalarIntegerOrNull(string query, Action<SqlCommand> sqlCommand = null);
        string SqlExecuteScalarStringOrNull(string query, Action<SqlCommand> sqlCommand = null);
        string SqlExecuteReader(string query, Func<SqlDataReader, string> map);

    }

    public sealed class SqlReader : ISqlReader 
    {
        public static int DEFAULT_SQL_TIMEOUT = 10;
        ISqlConnectionStringProvider _connectionStringProvider;

        public SqlReader(ISqlConnectionStringProvider sqlConnectionStringProvider)
        {
            _connectionStringProvider = sqlConnectionStringProvider ?? throw new ArgumentNullException(nameof(sqlConnectionStringProvider));
        }

        public List<T> SqlExecuteReader<T>(string query, Action<SqlCommand> sqlCommand, Func<SqlDataReader, T> map)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                sqlCommand?.Invoke(command);

                List<T> elements = null;

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        elements = new List<T>();

                    while (reader.Read())
                    {
                        elements.Add(map(reader));
                    }
                }

                return elements;
            }
        }

        public List<T> SqlExecuteReader<T>(string query, Func<SqlDataReader, T> map)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                List<T> elements = null;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        elements = new List<T>();

                    while (reader.Read())
                    {
                        elements.Add(map(reader));
                    }
                }

                return elements;
            }
        }

        public T SqlExecuteObjectReader<T>(string query, Func<SqlDataReader, T> map) where T : class , new()
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                T element = null;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        element = new T();

                    if (reader.Read())
                        element = map(reader);
                }

                return element;
            }
        }

        public string SqlExecuteReader(string query, Func<SqlDataReader, string> map)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                string element = string.Empty;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        if (reader.Read())
                            element = map(reader);
                }

                return element;
            }
        }

        public int SqlExecuteNonQuery(string query, Action<SqlCommand> sqlCommand = null)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                sqlCommand?.Invoke(command);

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows;
            }
        }

        public int? SqlExecuteScalarIntegerOrNull(string query, Action<SqlCommand> sqlCommand = null)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                sqlCommand?.Invoke(command);

                var result = command.ExecuteScalar();

                if (result != null && result.GetType() != typeof(DBNull))
                {
                    return Convert.ToInt32(result);
                }

                return null;
            }
        }

        public string SqlExecuteScalarStringOrNull(string query, Action<SqlCommand> sqlCommand = null)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = DEFAULT_SQL_TIMEOUT;

                sqlCommand?.Invoke(command);

                var result = command.ExecuteScalar();

                if (result != null && result.GetType() != typeof(DBNull))
                {
                    return result.ToString();
                }

                return null;
            }
        }

        private SqlConnection OpenConnection()
        {
            var con = new SqlConnection(_connectionStringProvider.GetConnectionString());
            con.Open();

            return con;
        }
    }
}
