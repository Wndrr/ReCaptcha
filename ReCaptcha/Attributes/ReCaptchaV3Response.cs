using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RelaxationPortal.Attributes
{
    public partial class ReCaptchaV3Response
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTimeOffset ChallengeTs { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }

    public partial class ReCaptchaV3Response
    {
        public static ReCaptchaV3Response FromJson(string json) => JsonConvert.DeserializeObject<ReCaptchaV3Response>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ReCaptchaV3Response self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}