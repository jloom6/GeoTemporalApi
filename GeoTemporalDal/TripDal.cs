using Dapper;
using GeoTemporalModels;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Linq;

namespace GeoTemporalDal
{
    public static class TripDal
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        public static Trip GetTrip(int tripId)
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                return dbConnection.GetTrip(tripId);
            }
        }

        private static Trip GetTrip(this IDbConnection dbConnection, int tripId)
        {
            return
                dbConnection.Query<Trip>("sp_get_trip", new {TripId = tripId}, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
        }

        public static Trip GetTripWithMessages(int tripId)
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                return dbConnection.GetTripWithMessages(tripId);
            }
        }

        private static Trip GetTripWithMessages(this IDbConnection dbConnection, int tripId)
        {
            var trip = dbConnection.GetTrip(tripId);
            if (trip != null)
                trip.Messages = dbConnection.GetTripMessages(tripId);
            return trip;
        }

        public static int GetTotalTripsOccuringAtTime(long epoch)
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                return
                    dbConnection.Query<int>("sp_get_total_trips_occuring_at_time", new { Epoch = epoch },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public static int GetTotalTripsThroughGeoRectangle(GeoRectangle geoRectangle)
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                return
                    dbConnection.Query<int>("sp_get_total_trips_through_georect",
                        new
                        {
                            MinLatitude = geoRectangle.BottomLeft.Latitude,
                            MinLongitude = geoRectangle.BottomLeft.Longitude,
                            MaxLatitude = geoRectangle.TopRight.Latitude,
                            MaxLongitude = geoRectangle.TopRight.Longitude
                        }, commandType: CommandType.StoredProcedure)
                        .FirstOrDefault();
            }
        }

        public static dynamic GetTotalTripsAndFaresThatStartOrStopInGeoRectangle(GeoRectangle geoRectangle)
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                return
                    dbConnection.Query<dynamic>("sp_get_total_trips_and_fares_that_start_or_stop_in_georect",
                        new
                        {
                            MinLatitude = geoRectangle.BottomLeft.Latitude,
                            MinLongitude = geoRectangle.BottomLeft.Longitude,
                            MaxLatitude = geoRectangle.TopRight.Latitude,
                            MaxLongitude = geoRectangle.TopRight.Longitude
                        }, commandType: CommandType.StoredProcedure)
                        .FirstOrDefault();
            }
        }

        public static void TruncateTrips()
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("sp_truncate_trips", commandType: CommandType.StoredProcedure);
            }
        }
    }
}
