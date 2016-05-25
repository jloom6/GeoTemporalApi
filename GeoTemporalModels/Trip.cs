using Newtonsoft.Json;

namespace GeoTemporalModels
{
    public class Trip
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("beginEpoch")]
        public long BeginEpoch { get; set; }

        [JsonProperty("endEppoch")]
        public long? EndEpoch { get; set; }

        [JsonProperty("fare")]
        public decimal? Fare { get; set; }

        [JsonProperty("lastMessageEpoch")]
        public long LastMessageEpoch { get; set; }

        [JsonProperty("lastLatitude")]
        public decimal LastLatitude { get; set; }

        [JsonProperty("lastLongitude")]
        public decimal LastLongitude { get; set; }

        [JsonProperty("messages")]
        public TripMessage[] Messages { get; set; }
    }
}
