using GeoTemporalBl;
using GeoTemporalModels;
using NUnit.Framework;

namespace GeoTemporalTests
{
    [TestFixture]
    public class GetTripPointInTimeQueryTests
    {
        [SetUp]
        [TearDown]
        public void CleanUp()
        {
            TripBl.TruncateTrips();
        }

        [Test]
        public void TestSingleTripPointInTimeQueryAfterBeginMessage()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 37,
                Longitude = 122,
                TripId = 432
            });
            var totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673020);
            Assert.That(totalTrips, Is.EqualTo(0));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673030);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673040);
            Assert.That(totalTrips, Is.EqualTo(1));
        }

        [Test]
        public void TestSingleTripPointInTimeQueryAfterUpdateMessage()
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
            var totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673020);
            Assert.That(totalTrips, Is.EqualTo(0));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673030);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673040);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673070);
            Assert.That(totalTrips, Is.EqualTo(1));
        }

        [Test]
        public void TestSingleTripPointInTimeQueryAfterEndMessage()
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
            var totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673020);
            Assert.That(totalTrips, Is.EqualTo(0));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673030);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673040);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673070);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673080);
            Assert.That(totalTrips, Is.EqualTo(0));
        }

        [Test]
        public void TestMultipleTripPointInTimeQuery()
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
                Latitude = 40,
                Longitude = 120,
                TripId = 432
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673040,
                Event = TripEvent.Begin,
                Latitude = 40,
                Longitude = 120,
                TripId = 433
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673070,
                Event = TripEvent.Update,
                Latitude = 39,
                Longitude = 124,
                TripId = 433
            });
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673080,
                Event = TripEvent.End,
                Fare = 50,
                Latitude = 50,
                Longitude = 130,
                TripId = 433
            });
            var totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673020);
            Assert.That(totalTrips, Is.EqualTo(0));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673030);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673040);
            Assert.That(totalTrips, Is.EqualTo(2));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673070);
            Assert.That(totalTrips, Is.EqualTo(2));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673080);
            Assert.That(totalTrips, Is.EqualTo(1));
            totalTrips = TripBl.GetTotalTripsOccuringAtTime(1392864673090);
            Assert.That(totalTrips, Is.EqualTo(0));
        }
    }
}
