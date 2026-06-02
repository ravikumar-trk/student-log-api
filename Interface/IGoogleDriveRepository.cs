using System.IO;
using System.Threading.Tasks;

namespace student_log_api.Interface
{
    public interface IGoogleDriveRepository
    {
        /// <summary>
        /// Uploads the provided stream to Google Drive.
        /// Returns (fileId, webViewLink) on success.
        /// </summary>
        Task<(string? fileId, string? webViewLink)> UploadFileAsync(Stream stream, string fileName, string contentType);
    }
}