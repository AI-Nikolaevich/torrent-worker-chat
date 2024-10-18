namespace Torrent.Chat.Worker.Configuration
{
    public class DatabaseConfig
    {
        public string? Server { get; set; }
        public int Port { get; set; }
        public string? Database { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public string ToConnectionString()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(Server));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(Database));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(Username));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(Password));

            if (Port == 0)
            {
                throw new ArgumentException("Port can't be 0", nameof(Port));
            }

            return $"Server={Server};Port={Port};Database={Database};Username={Username};Password={Password}";
        }
    }
}
