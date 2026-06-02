namespace student_log_api.Common
{
    public class AppSettings
    {
        public required string ConnectionString { get; set; }

        // Google Drive configuration
        // Path to service account JSON keyfile that the server can access
        public string? GoogleServiceAccountKeyFile { get; set; }

        // Optional folder id in Google Drive where uploaded files will be placed
        public string? GoogleDriveFolderId { get; set; }

        // JWT Configuration
        public string? JwtSecret { get; set; }
        public string? JwtIssuer { get; set; }
        public string? JwtAudience { get; set; }
        public int JwtExpirationMinutes { get; set; } = 60;
    }
}