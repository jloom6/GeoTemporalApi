using GeoTemporalBl;
using GeoTemporalModels;
using NUnit.Framework;

namespace GeoTemporalTests
{
    [TestFixture]
    public class InsertTripMessageTests
    {
        [SetUp]
        [TearDown]
        public void CleanUp()
        {
            TripBl.TruncateTrips();
        }

        [Test]
        public void TestInsertBeginMessage()
        {
            var tripMessage = InsertBeginMessage();
            var trip = TripBl.GetTrip(tripMessage.TripId);
            Assert.That(trip, Is.Not.Null);
            Assert.That(trip.BeginEpoch, Is.EqualTo(tripMessage.Epoch));
            Assert.That(trip.EndEpoch, Is.Null);
            Assert.That(trip.Fare, Is.Null);
            Assert.That(trip.Id, Is.EqualTo(tripMessage.TripId));
            Assert.That(trip.LastLatitude, Is.EqualTo(tripMessage.Latitude.Value));
            Assert.That(trip.LastLongitude, Is.EqualTo(tripMessage.Longitude.Value));
            Assert.That(trip.LastMessageEpoch, Is.EqualTo(tripMessage.Epoch));
        }

        private static TripMessage InsertBeginMessage()
        {
            var tripMessage = new TripMessage
            {
                Epoch = 1392864673030,
                Event = TripEvent.Begin,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            };
            TripMessageBl.InsertTripMessage(tripMessage);
            return tripMessage;
        }

        [Test]
        public void TestInsertUpdateMessage()
        {
            InsertBeginMessage();
            var tripMessage = InsertUpdateMessage();
            var trip = TripBl.GetTrip(tripMessage.TripId);
            Assert.That(trip, Is.Not.Null);
            Assert.That(trip.BeginEpoch, Is.Not.EqualTo(tripMessage.Epoch));
            Assert.That(trip.EndEpoch, Is.Null);
            Assert.That(trip.Fare, Is.Null);
            Assert.That(trip.Id, Is.EqualTo(tripMessage.TripId));
            Assert.That(trip.LastLatitude, Is.EqualTo(tripMessage.Latitude.Value));
            Assert.That(trip.LastLongitude, Is.EqualTo(tripMessage.Longitude.Value));
            Assert.That(trip.LastMessageEpoch, Is.EqualTo(tripMessage.Epoch));
        }

        private static TripMessage InsertUpdateMessage()
        {
            var tripMessage = new TripMessage
            {
                Epoch = 1392864673040,
                Event = TripEvent.Update,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            };
            TripMessageBl.InsertTripMessage(tripMessage);
            return tripMessage;
        }

        [Test]
        public void TestInsertEndMessage()
        {
            InsertBeginMessage();
            InsertUpdateMessage();
            var tripMessage = InsertEndMessage();
            var trip = TripBl.GetTrip(tripMessage.TripId);
            Assert.That(trip, Is.Not.Null);
            Assert.That(trip.BeginEpoch, Is.Not.EqualTo(tripMessage.Epoch));
            Assert.That(trip.EndEpoch, Is.EqualTo(tripMessage.Epoch));
            Assert.That(trip.Fare, Is.EqualTo(tripMessage.Fare));
            Assert.That(trip.Id, Is.EqualTo(tripMessage.TripId));
            Assert.That(trip.LastLatitude, Is.EqualTo(tripMessage.Latitude.Value));
            Assert.That(trip.LastLongitude, Is.EqualTo(tripMessage.Longitude.Value));
            Assert.That(trip.LastMessageEpoch, Is.EqualTo(tripMessage.Epoch));
        }

        private static TripMessage InsertEndMessage()
        {
            var tripMessage = new TripMessage
            {
                Epoch = 1392864673070,
                Event = TripEvent.End,
                Fare = 25,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            };
            TripMessageBl.InsertTripMessage(tripMessage);
            return tripMessage;
        }
    }
}
