using Npgsql;
namespace Torrent.Chat.Storage
{
    public class ChatContext(NpgsqlDataSource dataSource)
    : ContextBase(dataSource)
    {
    }
}
