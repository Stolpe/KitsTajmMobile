using System;
using Newtonsoft.Json;

namespace KitsTajmMobile.Service
{
    public class LoginRequest
    {
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
    }

    public class PostNewTimeRecordsRequest
    {
        [JsonProperty(PropertyName = "activity")]
        public int ActivityId { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int? RecordId { get; set; }
        [JsonProperty(PropertyName = "project")]
        public int ProjectId { get; set; }
        [JsonProperty(PropertyName = "time")]
        public int Time { get; set; }
    }

    public class PutTimeRecordsRequest
    {
        [JsonProperty(PropertyName = "activity")]
        public int ActivityId { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int RecordId { get; set; }
        [JsonProperty(PropertyName = "project")]
        public int ProjectId { get; set; }
        [JsonProperty(PropertyName = "time")]
        public int Time { get; set; }
    }
}
