using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KitsTajmMobile.Service
{
    public class LoginResponse
    {
        [JsonProperty(PropertyName = "code")]
        public LoginResponseCode Code { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "role")]
        public LoginResponseRole Role { get; set; }
        [JsonProperty(PropertyName = "slack")]
        public string Slack { get; set; }

        public enum LoginResponseCode
        {
            UnableToLogin = 111,
            LoggedInOk = 112
        }

        public enum LoginResponseRole
        {
            [JsonProperty(PropertyName = "n")]
            N
        }
    }

    public class ProjectResponse
    {
        [JsonProperty(PropertyName = "activities")]
        public IEnumerable<Activity> Activities { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public class Activity
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }
    }

    public class GetTimeRecordsResponse
    {
        [JsonProperty(PropertyName = "activity")]
        public string Activity { get; set; }
        [JsonProperty(PropertyName = "activity_id")]
        public int ActivityId { get; set; }
        [JsonProperty(PropertyName = "customername")]
        public string CustomerName { get; set; }
        [JsonProperty(PropertyName = "project_id")]
        public int ProjectId { get; set; }
        [JsonProperty(PropertyName = "project_name")]
        public string ProjectName { get; set; }
        [JsonProperty(PropertyName = "record_id")]
        public int RecordId { get; set; }
        [JsonProperty(PropertyName = "reporteddate")]
        public DateTime ReportedDate { get; set; }
        [JsonProperty(PropertyName = "statement_id")]
        public int? StatementId { get; set; }
        [JsonProperty(PropertyName = "time")]
        public int Time { get; set; }
    }

    public class PostNewTimeRecordResponse : WriteResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int RecordId { get; set; }

        public override bool IsSuccess
        {
            get
            {
                return this.RecordId != 0;
            }
        }
    }

    public class PutTimeRecordResponse : WriteResponse
    {
        [JsonProperty(PropertyName = "res")]
        [JsonConverter(typeof(ResultResponseCodeConverter))]
        public ResultResposeCode Result { get; set; }

        public override bool IsSuccess
        {
            get
            {
                return this.Result == ResultResposeCode.Ok;
            }
        }

        public enum ResultResposeCode
        {
            NotOk,
            Ok
        }

        private class ResultResponseCodeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(string);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                switch ((string)reader.Value)
                {
                    case "nok":
                        return ResultResposeCode.NotOk;
                    case "ok":
                        return ResultResposeCode.Ok;
                    default:
                        throw new JsonSerializationException($"Unexpected enum member: {reader.Value}");
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                switch ((ResultResposeCode)value)
                {
                    case ResultResposeCode.NotOk:
                        writer.WriteValue("nok");
                        break;
                    case ResultResposeCode.Ok:
                        writer.WriteValue("ok");
                        break;
                }
            }
        }
    }

    public class DeleteTimeRecordResponse : WriteResponse
    {
        [JsonProperty(PropertyName = "res")]
        [JsonConverter(typeof(ResultResponseCodeConverter))]
        public ResultResposeCode Result { get; set; }

        public override bool IsSuccess
        {
            get
            {
                return this.Result == ResultResposeCode.Ok;
            }
        }

        public enum ResultResposeCode
        {
            NotOk,
            Ok
        }

        private class ResultResponseCodeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(string);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                switch ((string)reader.Value)
                {
                    case "nok":
                        return ResultResposeCode.NotOk;
                    case "ok":
                        return ResultResposeCode.Ok;
                    default:
                        throw new JsonSerializationException($"Unexpected enum member: {reader.Value}");
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                switch ((ResultResposeCode)value)
                {
                    case ResultResposeCode.NotOk:
                        writer.WriteValue("nok");
                        break;
                    case ResultResposeCode.Ok:
                        writer.WriteValue("ok");
                        break;
                }
            }
        }
    }

    public abstract class WriteResponse
    {
        public abstract bool IsSuccess { get; }
    }
}
