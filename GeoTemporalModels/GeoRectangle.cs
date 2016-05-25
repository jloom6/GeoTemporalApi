using Newtonsoft.Json;

namespace GeoTemporalModels
{
    public class GeoRectangle
    {
        public GeoRectangle(GeoPoint bottomLeft, GeoPoint topRight)
        {
            BottomLeft = bottomLeft;
            TopRight = topRight;
        }

        [JsonProperty("bottomLeft")]
        public GeoPoint BottomLeft { get; set; }

        [JsonProperty("topRight")]
        public GeoPoint TopRight { get; set; }

        public void Validate()
        {
            BottomLeft.Validate();
            TopRight.Validate();
            ValidateLatitudes();
            ValidateLongitudes();
        }

        private void ValidateLatitudes()
        {
            if (TopRight.Latitude < BottomLeft.Latitude)
                throw new InvalidGeoRectangleException(
                    "The BottomLeft Latitude cannot be greater than the TopRight Latitude.");
        }

        private void ValidateLongitudes()
        {
            if (TopRight.Longitude < BottomLeft.Longitude)
                throw new InvalidGeoRectangleException(
                    "The BottomLeft Longitude cannot be greater than the TopRight Longitude.");
        }
    }
}
