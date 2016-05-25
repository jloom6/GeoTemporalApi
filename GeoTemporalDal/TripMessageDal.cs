using Dapper;
using GeoTemporalModels;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Linq;

namespace GeoTemporalDal
{
    public static class TripMessageDal
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        public static void InsertTripMessage(TripMessage tripMessage)
        {
            using (var dbConnection = new MySqlConnection(ConnectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("sp_insert_trip_message",
                    new
                    {
                        EventId = (int)tripMessage.Event.Value,
                        tripMessage.TripId,
                        tripMessage.Latitude,
                        tripMessage.Longitude,
                        tripMessage.Epoch,
                        tripMessage.Fare
                    }, commandType: CommandType.StoredProcedure);
            }
        }

        internal static TripMessage[] GetTripMessages(this IDbConnection dbConnection, int tripId)
        {
            return
                dbConnection.Query<TripMessage>("sp_get_trip_messages", new {TripId = tripId},
                    commandType: CommandType.StoredProcedure).ToArray();
        }
    }
}
