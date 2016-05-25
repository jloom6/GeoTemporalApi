using GeoTemporalDal;
using GeoTemporalModels;

namespace GeoTemporalBl
{
    public static class TripMessageBl
    {
        public static void InsertTripMessage(TripMessage tripMessage)
        {
            ValidateTripMessage(tripMessage);
            TripMessageDal.InsertTripMessage(tripMessage);
        }

        private static void ValidateTripMessage(TripMessage tripMessage)
        {
            tripMessage.Validate();
            var trip = TripDal.GetTrip(tripMessage.TripId);
            if (TripEvent.Begin == tripMessage.Event)
            {
                if (trip != null)
                    throw new TripAlreadyExistsException(
                        "Trip with ID {0} already exists, therefore a begin message cannot be saved.",
                        tripMessage.TripId);
            }
            else
            {
                if (null == trip)
                    throw new TripDoesNotExistException(
                        "Trip with ID {0} does not exist, therefore a non begin message cannot be saved.",
                        tripMessage.TripId);
                if (trip.LastMessageEpoch >= tripMessage.Epoch)
                    throw new TripAlreadyHasMessageAfterEpochException(
                        "Trip with ID {0} already has an update after the messages epoch, therefore a non begin message cannot be saved.",
                        tripMessage.TripId);
                if (trip.EndEpoch.HasValue)
                    throw new TripAlreadyEndedException(
                        "Trip with ID {0} has already ended, therefore a message can not be saved.", tripMessage.TripId);
            }
        }
    }
}
