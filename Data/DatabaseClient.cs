using System;
using System.Data;
using Npgsql;

namespace GymManagementClient.Data
{
    public sealed class DatabaseClient
    {
        private readonly string _connectionString;

        public DatabaseClient()
        {
            if (string.IsNullOrEmpty(SessionContext.ConnectionString))
            {
                throw new InvalidOperationException("Строка подключения не задана. Войдите в систему перед использованием.");
            }

            _connectionString = SessionContext.ConnectionString;
        }

        public DataTable ExecuteQuery(string queryText, params NpgsqlParameter[] parameters)
        {
            var resultTable = new DataTable();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(queryText, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
            }

            return resultTable;
        }

        public int ExecuteNonQuery(string commandText, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
