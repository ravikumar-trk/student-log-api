using System.Text.Json.Serialization;

namespace student_log_api.Common
{
    public class ServiceResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        [JsonIgnore]
        public bool HasErrors { get { return Errors != null && Errors.Count > 0; } }
        [JsonIgnore]
        public bool HasWarnings { get { return Warnings != null && Warnings.Count > 0; } }

        public void addError(string error)
        {
            if (false == string.IsNullOrEmpty(error))
            {
                Errors.Add(error);
            }
        }

        public void addWarning(string warning)
        {
            if (false == string.IsNullOrEmpty(warning))
            {
                Warnings.Add(warning);
            }
        }

        public string GetErrors()
        {
            string result = null;

            if (Errors.Count > 0)
            {
                result = string.Join(Environment.NewLine, Errors);
            }

            return result;
        }

        public string GetWarnings()
        {
            string result = null;

            if (Warnings.Count > 0)
            {
                result = string.Join(Environment.NewLine, Warnings);
            }

            return result;
        }

        public string Message { get; set; } = "";
    }

    public class Payload<T> : ServiceResponse
    {
        public T Result { get; set; }
        public string ResponseCode { get; set; } = "";
    }
}
