using Torrent.Chat.Storage;
using Torrent.Chat.Worker.DbEntities;

namespace Torrent.Chat.Worker.Repositories
{
    public class DbRepository(ChatContext dbContext)
    {
        private readonly ChatContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task InsertMessagesAsync(ChatEntity chat)
        {
            ArgumentNullException.ThrowIfNull(chat, nameof(chat));

            string query = @"INSERT INTO Chat (username, message) VALUES (@username, @message);";

            await _dbContext.ExecuteAsync(query, new { username = chat.UserName, message = chat.Message });
        }
    }
}
