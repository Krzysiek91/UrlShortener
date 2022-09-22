using Microsoft.Data.Sqlite;

namespace UrlShortener.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly string _dbConnection;

        public UrlRepository(string dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<string> GetUrlByIdAsync(long id)
        {
            using (var connection = new SqliteConnection(_dbConnection))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT URL FROM URLs WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var url = string.Empty;
                    while (await reader.ReadAsync())
                    {
                        url = reader.GetString(0);
                    }

                    connection.Close();

                    return url;
                }
            }
        }

        public async Task<long> GetIdByUrlAsync(string url)
        {
            using (var connection = new SqliteConnection(_dbConnection))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id FROM URLs WHERE URL = $url";
                command.Parameters.AddWithValue("$url", url);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var id = 0L;
                    while (await reader.ReadAsync())
                    {
                        id = reader.GetInt64(0);
                    }

                    connection.Close();

                    return id;
                }
            }
        }

        public async Task<long> SaveUrlAsync(string url)
        {
            using (var connection = new SqliteConnection(_dbConnection))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO URLs (URL) VALUES ($url); select last_insert_rowid();";
                command.Parameters.AddWithValue("$url", url);
                var id = await command.ExecuteScalarAsync();

                connection.Close();

                if (id != null)
                {
                    return (long)id;
                }

                return 0;
            }
        }

        public async Task ClearDatabase(string dbConnection)
        {
            using (var connection = new SqliteConnection(dbConnection))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM URLs;";
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
