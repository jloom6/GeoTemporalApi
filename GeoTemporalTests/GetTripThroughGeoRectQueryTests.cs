using GeoTemporalBl;
using GeoTemporalModels;
using NUnit.Framework;

namespace GeoTemporalTests
{
    [TestFixture]
    public class GetTripThroughGeoRectQueryTests
    {
        [SetUp]
        [TearDown]
        public void CleanUp()
        {
            TripBl.TruncateTrips();
        }

        [Test]
        public void TestSingleTripThroughGeoRectQueryAfterBeginMessage()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 37,
                Longitude = 122,
                TripId = 432
            });
            var totalTrips = TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(36, 121), new GeoPoint(38, 123)));
            Assert.That(totalTrips, Is.EqualTo(1));
        }

        [Test]
        public void TestSingleTripThroughGeoRectQueryAfterUpdateMessage()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 37,
                Longitude = 122,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673040,
                Event = TripEvent.Update,
                Latitude = 39,
                Longitude = 124,
                TripId = 432
            });
            var totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(36, 121), new GeoPoint(38, 123)));
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(38, 123), new GeoPoint(40, 125)));
            Assert.That(totalTrips, Is.EqualTo(1));
        }

        [Test]
        public void TestSingleTripThroughGeoRectQueryAfterEndMessage()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 37,
                Longitude = 122,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673040,
                Event = TripEvent.Update,
                Latitude = 39,
                Longitude = 124,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673070,
                Event = TripEvent.End,
                Fare = 25,
                Latitude = 41,
                Longitude = 126,
                TripId = 432
            });
            var totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(36, 121), new GeoPoint(38, 123)));
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(38, 123), new GeoPoint(40, 125)));
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(40, 125), new GeoPoint(42, 127)));
            Assert.That(totalTrips, Is.EqualTo(1));
        }

        [Test]
        public void TestEntireTripContainedInGeoRectThroughGeoRectQuery()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 37,
                Longitude = 122,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673070,
                Event = TripEvent.End,
                Fare = 25,
                Latitude = 38,
                Longitude = 123,
                TripId = 432
            });
            var totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(36, 121), new GeoPoint(39, 124)));
            Assert.That(totalTrips, Is.EqualTo(1));
        }

        [Test]
        public void TestMultipleTripsThroughGeoRectQuery()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 30,
                Longitude = 110,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673070,
                Event = TripEvent.End,
                Fare = 25,
                Latitude = 40,
                Longitude = 120,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 40,
                Longitude = 120,
                TripId = 433
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673070,
                Event = TripEvent.End,
                Fare = 50,
                Latitude = 50,
                Longitude = 130,
                TripId = 433
            });
            var totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(29, 109), new GeoPoint(31, 111)));
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(49, 129), new GeoPoint(51, 131)));
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(39, 119), new GeoPoint(41, 121)));
            Assert.That(totalTrips, Is.EqualTo(2));
        }

        [Test]
        public void TestTripOnCornerThroughGeoRectQuery()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 30,
                Longitude = 120,
                TripId = 432
            });
            var totalTrips =
                TripBl.GetTotalTripsThroughGeoRectangle(new GeoRectangle(new GeoPoint(30, 120), new GeoPoint(40, 130)));
            Assert.That(totalTrips, Is.EqualTo(1));
        }
    }
}
