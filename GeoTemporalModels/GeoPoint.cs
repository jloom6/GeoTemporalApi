using Newtonsoft.Json;

namespace GeoTemporalModels
{
    public class GeoPoint
    {
        public GeoPoint(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        public void Validate()
        {
            ValidateLatitude();
            ValidateLongitude();
        }

        private void ValidateLatitude()
        {
            if (Latitude < -90 || Latitude > 90)
                throw new InvalidGeoPointException("Latitude must be between -90 and 90.");
        }

        private void ValidateLongitude()
        {
            if (Longitude < -180 || Longitude > 180)
                throw new InvalidGeoPointException("Longitude must be between -180 and 180.");
        }
    }
}
