using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;
using student_log_api.DBLibrary;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace student_log_api.Services
{
    public class TicketServices : ITicketInterface
    {
        public AppSettings AppSettings { get; }
        private readonly IGoogleDriveRepository _driveRepo;

        public TicketServices(IOptions<AppSettings> appSettings, IGoogleDriveRepository driveRepo)
        {
            AppSettings = appSettings.Value;
            _driveRepo = driveRepo;
        }

        public async Task<ServiceResponse> CreateTicket(CreateTicketPayloadV2 items)
        {
            ServiceResponse response = new();
            try
            {
                if (items == null)
                {
                    response.addWarning("Request data is missing.");
                    return response;
                }

                var validations = new List<(bool Condition, string Message)>
                {
                    (string.IsNullOrEmpty(items?.IssueType), "Issue Type is required."),
                    (string.IsNullOrEmpty(items?.Priority), "Priority is required."),
                    (string.IsNullOrEmpty(items?.Description), "Description is required."),
                    (items?.SchoolID <= 0, "Valid SchoolID is required."),
                    (items?.AccountID <= 0, "Valid AccountID is required."),
                    (items?.CreatedBy <= 0, "CreatedBy is required."),
                    (string.IsNullOrEmpty(items?.AccountCode), "AccountCode is required."),
                    (items?.ExcelFile == null, "Excel file is required."),
                    (items?.ExcelFile != null && items.ExcelFile.Length == 0, "Uploaded Excel file is empty.")
                };

                foreach (var validation in validations)
                {
                    if (validation.Condition)
                    {
                        response.addWarning(validation.Message);
                    }
                }

                if (response.HasWarnings)
                {
                    return response;
                }
                var sqlParams = new Dictionary<string, object>
                {
                    {"IssueType",items.IssueType},
                    {"Priority",items.Priority},
                    {"Description",items.Description},
                    {"AccountID",items.AccountID},
                    {"AccountCode",items.AccountCode},
                    {"SchoolID",items.SchoolID},
                    {"FileName", items.ExcelFile.FileName },
                    { "CreatedBy",items.CreatedBy},
                    { "CreatedOn",items.CreatedOn},

                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.INSERT_NEW_TICKET, sqlParams);

                if (string.IsNullOrEmpty(Result))
                {
                    response.Message = "Ticket not created.";
                }
                List<CreateTicketResponse> DeserializedResult = JsonConvert.DeserializeObject<List<CreateTicketResponse>>(Result);
                if (DeserializedResult == null || DeserializedResult.Count == 0)
                {
                    response.Message = "No data found.";
                }
                else
                {
                    response.Message = "Ticket created but with some issues: " + DeserializedResult[0].TicketName;
                }


                // var ext = Path.GetExtension(model.ExcelFile.FileName);
                // var fileName = $"{Guid.NewGuid()}{ext}";

                // using (var stream = model.ExcelFile.OpenReadStream())
                // {
                //     // Upload to Google Drive
                //     var (fileId, webLink) = await _driveRepo.UploadFileAsync(stream, fileName, model.ExcelFile.ContentType ?? "application/octet-stream");

                //     if (string.IsNullOrEmpty(fileId))
                //     {
                //         response.addError("Upload to Google Drive failed");
                //         return response;
                //     }

                //     // TODO: Add Excel parsing / DB handling here (call DB/utility as required)

                //     response.Message = "File uploaded successfully";
                //     if (!string.IsNullOrEmpty(webLink))
                //     {
                //         response.Message += $". DriveLink: {webLink}";
                //     }
                // }
            }
            catch (SqlException e)
            {
                response.addError(e.Message);
            }
            catch (ArgumentNullException e)
            {
                response.addError(e.Message);
            }
            catch (Exception e)
            {
                response.addError(e.Message);
            }
            return response;
        }
    }
}