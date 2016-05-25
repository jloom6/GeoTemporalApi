using GeoTemporalBl;
using GeoTemporalModels;
using NUnit.Framework;
using System;

namespace GeoTemporalTests
{
    [TestFixture]
    public class GetTripQueryExceptionTests
    {
        [Test]
        public void TestGetTripQueryWithInvalidEpoch()
        {
            var exception = Assert.Throws<ArgumentException>(GetTripQueryWithInvalidEpoch);
            Assert.That(exception.Message, Is.EqualTo("Epoch must be an integer greater than 0.\r\nParameter name: epoch"));
            Assert.That(exception.ParamName, Is.EqualTo("epoch"));
        }

        private static void GetTripQueryWithInvalidEpoch()
        {
            TripBl.GetTotalTripsOccuringAtTime(0);
        }

        [Test]
        public void TestGetTripQueryWithInvalidGeoPointLatitude()
        {
            var exception = Assert.Throws<InvalidGeoPointException>(GetTripQueryWithInvalidGeoPointLatitude);
            Assert.That(exception.Message, Is.EqualTo("Latitude must be between -90 and 90."));
        }

        private static void GetTripQueryWithInvalidGeoPointLatitude()
        {
            TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(-91, 0), new GeoPoint(37, 122)));
        }

        [Test]
        public void TestGetTripQueryWithInvalidGeoPointLongitude()
        {
            var exception = Assert.Throws<InvalidGeoPointException>(GetTripQueryWithInvalidGeoPointLongitude);
            Assert.That(exception.Message, Is.EqualTo("Longitude must be between -180 and 180."));
        }

        private static void GetTripQueryWithInvalidGeoPointLongitude()
        {
            TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(-37, 122), new GeoPoint(0, 181)));
        }

        [Test]
        public void TestGetTripQueryWithInvalidGeoRectangleLatitudes()
        {
            var exception = Assert.Throws<InvalidGeoRectangleException>(GetTripQueryWithInvalidGeoRectangleLatitudes);
            Assert.That(exception.Message,
                Is.EqualTo("The BottomLeft Latitude cannot be greater than the TopRight Latitude."));
        }

        private static void GetTripQueryWithInvalidGeoRectangleLatitudes()
        {
            TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(50, 122), new GeoPoint(20, 123)));
        }

        [Test]
        public void TestGetTripQueryWithInvalidGeoRectangleLongitudes()
        {
            var exception = Assert.Throws<InvalidGeoRectangleException>(GetTripQueryWithInvalidGeoRectangleLongitudes);
            Assert.That(exception.Message,
                Is.EqualTo("The BottomLeft Longitude cannot be greater than the TopRight Longitude."));
        }

        private static void GetTripQueryWithInvalidGeoRectangleLongitudes()
        {
            TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(50, 122), new GeoPoint(60, 100)));
        }
    }
}
