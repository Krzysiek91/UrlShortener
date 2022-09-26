using FluentAssertions;
using Microsoft.Data.Sqlite;
using UrlShortener.Repositories;

namespace UrlShortener.Tests.Repositories
{
    public class UrlRepositoryIntegrationTests
    {
        [Fact]
        public async Task SaveUrlAsync_ShouldInsertUrlToSqliteDatabase()
        {
            var dbConnection = "Data Source=IntegrationDatabase\\UrlShortenerDB.db";
            var repo = new UrlRepository(dbConnection);

            await repo.ClearDatabase(dbConnection);

            var idSave = await repo.SaveUrlAsync("https://test.com");
            var idRetrive = await repo.GetIdByUrlAsync("https://test.com");
            var url = await repo.GetUrlByIdAsync(idRetrive);

            idSave.Should().Be(idRetrive);
            url.Should().Be("https://test.com");

            await repo.ClearDatabase(dbConnection);
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
