using GeoTemporalBl;
using GeoTemporalModels;
using NUnit.Framework;

namespace GeoTemporalTests
{
    [TestFixture]
    public class InsertTripMessageExceptionTests
    {
        [SetUp]
        [TearDown]
        public void CleanUp()
        {
            TripBl.TruncateTrips();
        }

        [Test]
        public void TestInsertTripMessageWithMissingEvent()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertTripMessageWithMissingEvent);
            Assert.That(exception.Message, Is.EqualTo("Event cannot be null."));
        }

        private static void InsertTripMessageWithMissingEvent()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertTripMessageWithInvalidTripId()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertTripMessageWithInvalidTripId);
            Assert.That(exception.Message, Is.EqualTo("TripId must be an integer greater than 0."));
        }

        private static void InsertTripMessageWithInvalidTripId()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 0
            });
        }

        [Test]
        public void TestInsertTripMessageWithMissingLatitude()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertTripMessageWithMissingLatitude);
            Assert.That(exception.Message, Is.EqualTo("Latitude cannot be null."));
        }

        private static void InsertTripMessageWithMissingLatitude()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertTripMessageWithMissingLongitude()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertTripMessageWithMissingLongitude);
            Assert.That(exception.Message, Is.EqualTo("Longitude cannot be null."));
        }

        private static void InsertTripMessageWithMissingLongitude()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertTripMessageWithInvalidLatitude()
        {
            var exception = Assert.Throws<InvalidGeoPointException>(InsertTripMessageWithInvalidLatitude);
            Assert.That(exception.Message, Is.EqualTo("Latitude must be between -90 and 90."));
        }

        private static void InsertTripMessageWithInvalidLatitude()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 91,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertTripMessageWithInvalidLongitude()
        {
            var exception = Assert.Throws<InvalidGeoPointException>(InsertTripMessageWithInvalidLongitude);
            Assert.That(exception.Message, Is.EqualTo("Longitude must be between -180 and 180."));
        }

        private static void InsertTripMessageWithInvalidLongitude()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 181,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertEndTripMessageWithMissingFare()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertEndTripMessageWithMissingFare);
            Assert.That(exception.Message, Is.EqualTo("End trip message must contain fare."));
        }

        private static void InsertEndTripMessageWithMissingFare()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.End,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertNonEndTripMessageWithFare()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertNonEndTripMessageWithFare);
            Assert.That(exception.Message, Is.EqualTo("Non End trip message cannot contain fare."));
        }

        private static void InsertNonEndTripMessageWithFare()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Fare = 25,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertTripMessageWithInvalidEpoch()
        {
            var exception = Assert.Throws<InvalidTripMessageException>(InsertTripMessageWithInvalidEpoch);
            Assert.That(exception.Message, Is.EqualTo("Epoch must be an integer greater than 0."));
        }

        private static void InsertTripMessageWithInvalidEpoch()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 0,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertBeginTripMessageThatAlreadyExists()
        {
            var exception = Assert.Throws<TripAlreadyExistsException>(InsertBeginTripMessageThatAlreadyExists);
            Assert.That(exception.Message,
                Is.EqualTo("Trip with ID 432 already exists, therefore a begin message cannot be saved."));
        }

        private static void InsertBeginTripMessageThatAlreadyExists()
        {
            var tripMessage = new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            };
            TripMessageBl.InsertTripMessage(tripMessage);
            TripMessageBl.InsertTripMessage(tripMessage);
        }

        [Test]
        public void TestInsertNonBeginTripMessageThatDoesNotExist()
        {
            var exception = Assert.Throws<TripDoesNotExistException>(InsertNonBeginTripMessageThatDoesNotExist);
            Assert.That(exception.Message,
                Is.EqualTo("Trip with ID 432 does not exist, therefore a non begin message cannot be saved."));
        }

        private static void InsertNonBeginTripMessageThatDoesNotExist()
        {
            TripMessageBl.InsertTripMessage(new TripMessage
            {
                Event = TripEvent.Update,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            });
        }

        [Test]
        public void TestInsertTripMessageThatAlreadyHasMessageAfterEpoch()
        {
            var exception =
                Assert.Throws<TripAlreadyHasMessageAfterEpochException>(InsertTripMessageThatAlreadyHasMessageAfterEpoch);
            Assert.That(exception.Message,
                Is.EqualTo(
                    "Trip with ID 432 already has an update after the messages epoch, therefore a non begin message cannot be saved."));
        }

        private static void InsertTripMessageThatAlreadyHasMessageAfterEpoch()
        {
            var tripMessage = new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            };
            TripMessageBl.InsertTripMessage(tripMessage);
            tripMessage.Epoch = 1392864673020;
            tripMessage.Event = TripEvent.Update;
            TripMessageBl.InsertTripMessage(tripMessage);
        }

        [Test]
        public void TestInsertTripMessageThatAlreadyEnded()
        {
            var exception = Assert.Throws<TripAlreadyEndedException>(InsertTripMessageThatAlreadyEnded);
            Assert.That(exception.Message,
                Is.EqualTo("Trip with ID 432 has already ended, therefore a message can not be saved."));
        }

        private static void InsertTripMessageThatAlreadyEnded()
        {
            var tripMessage = new TripMessage
            {
                Event = TripEvent.Begin,
                Epoch = 1392864673030,
                Latitude = 37.79947m,
                Longitude = 122.51163m,
                TripId = 432
            };
            TripMessageBl.InsertTripMessage(tripMessage);
            tripMessage.Epoch = 1392864673070;
            tripMessage.Fare = 25;
            tripMessage.Event = TripEvent.End;
            TripMessageBl.InsertTripMessage(tripMessage);
            tripMessage.Epoch = 1392864673080;
            tripMessage.Event = TripEvent.Update;
            tripMessage.Fare = null;
            TripMessageBl.InsertTripMessage(tripMessage);
        }
    }
}
