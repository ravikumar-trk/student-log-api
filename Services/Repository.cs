using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.Extensions.Options;
using student_log_api.Common;
using student_log_api.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace student_log_api.Services
{
    public class Repository : IGoogleDriveRepository
    {
        private readonly AppSettings _settings;

        public Repository(IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value;
        }

        public async Task<(string? fileId, string? webViewLink)> UploadFileAsync(Stream stream, string fileName, string contentType)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrEmpty(_settings.GoogleServiceAccountKeyFile))
            {
                throw new InvalidOperationException("GoogleServiceAccountKeyFile not configured in AppSettings.");
            }

            GoogleCredential credential;
            try
            {
                credential = GoogleCredential.FromFile(_settings.GoogleServiceAccountKeyFile)
                    .CreateScoped(DriveService.Scope.Drive);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not load Google service account keyfile.", e);
            }

            using var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "student-log-api"
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
            };

            if (!string.IsNullOrEmpty(_settings.GoogleDriveFolderId))
            {
                fileMetadata.Parents = new string[] { _settings.GoogleDriveFolderId };
            }

            var request = driveService.Files.Create(fileMetadata, stream, contentType);
            request.Fields = "id,webViewLink,webContentLink";

            var uploadProgress = await request.UploadAsync();

            if (uploadProgress.Exception != null)
            {
                throw uploadProgress.Exception;
            }

            var file = request.ResponseBody;

            // Optionally make file accessible via link (anyone with link)
            try
            {
                var permission = new Permission()
                {
                    Role = "reader",
                    Type = "anyone"
                };
                await driveService.Permissions.Create(permission, file.Id).ExecuteAsync();
            }
            catch
            {
                // swallow permission errors; upload succeeded, but link may be restricted
            }

            // If webViewLink is empty, try a Get call to fetch it
            if (string.IsNullOrEmpty(file.WebViewLink))
            {
                var getReq = driveService.Files.Get(file.Id);
                getReq.Fields = "id,webViewLink,webContentLink";
                file = await getReq.ExecuteAsync();
            }

            return (file.Id, file.WebViewLink ?? file.WebContentLink);
        }
    }
}
