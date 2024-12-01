using Newtonsoft.Json;

namespace HealthBook.Utility
{
    public class BaseResponseModel
    {
        public BaseResponseModel()
        {
            Data = null;
            Message = null;
        }

        [JsonProperty("Data")]
        public object Data { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("StatusCode")]
        public long StatusCode { get; set; }
    }
}
