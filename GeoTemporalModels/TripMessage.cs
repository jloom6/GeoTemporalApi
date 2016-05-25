using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeoTemporalModels
{
    public class TripMessage
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TripEvent? Event { get; set; }

        [JsonProperty("tripId")]
        public int TripId { get; set; }

        [JsonProperty("lat")]
        public decimal? Latitude { get; set; }

        [JsonProperty("lng")]
        public decimal? Longitude { get; set; }

        [JsonProperty("fare")]
        public decimal? Fare { get; set; }

        [JsonProperty("epoch")]
        public long Epoch { get; set; }

        public void Validate()
        {
            ValidateEvent();
            ValidateTripId();
            ValidateLatitudeAndLongitude();
            ValidateFare();
            ValidateEpoch();
        }

        private void ValidateEvent()
        {
            if (null == Event)
                throw new InvalidTripMessageException("Event cannot be null.");
        }

        private void ValidateTripId()
        {
            if (TripId <= 0)
                throw new InvalidTripMessageException("TripId must be an integer greater than 0.");
        }

        private void ValidateLatitudeAndLongitude()
        {
            if (!Latitude.HasValue)
                throw new InvalidTripMessageException("Latitude cannot be null.");
            if (!Longitude.HasValue)
                throw new InvalidTripMessageException("Longitude cannot be null.");
            new GeoPoint(Latitude.Value, Longitude.Value).Validate();
        }

        private void ValidateFare()
        {
            if (TripEvent.End == Event)
            {
                if (!Fare.HasValue)
                    throw new InvalidTripMessageException("End trip message must contain fare.");
            }
            else if (Fare.HasValue)
                throw new InvalidTripMessageException("Non End trip message cannot contain fare.");
        }

        private void ValidateEpoch()
        {
            if (Epoch <= 0)
                throw new InvalidTripMessageException("Epoch must be an integer greater than 0.");
        }
    }
}
