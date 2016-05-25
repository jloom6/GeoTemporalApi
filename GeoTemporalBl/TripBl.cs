using GeoTemporalDal;
using GeoTemporalModels;
using System;

namespace GeoTemporalBl
{
    public static class TripBl
    {
        public static Trip GetTrip(int tripId)
        {
            return TripDal.GetTrip(tripId);
        }

        public static Trip GetTripWithMessages(int tripId)
        {
            return TripDal.GetTripWithMessages(tripId);
        }

        public static int GetTotalTripsOccuringAtTime(long epoch)
        {
            if (epoch <= 0)
                throw new ArgumentException("Epoch must be an integer greater than 0.", "epoch");
            return TripDal.GetTotalTripsOccuringAtTime(epoch);
        }

        public static int GetTotalTripsThroughGeoRectangle(GeoRectangle geoRectangle)
        {
            geoRectangle.Validate();
            return TripDal.GetTotalTripsThroughGeoRectangle(geoRectangle);
        }

        public static dynamic GetTotalTripsAndFaresThatStartOrStopInGeoRectangle(GeoRectangle geoRectangle)
        {
            geoRectangle.Validate();
            return TripDal.GetTotalTripsAndFaresThatStartOrStopInGeoRectangle(geoRectangle);
        }

        public static void TruncateTrips()
        {
            TripDal.TruncateTrips();
        }
    }
}
