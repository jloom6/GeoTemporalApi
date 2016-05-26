using GeoTemporalBl;
using GeoTemporalModels;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace GeoTemporalApi
{
    public class TripModule : NancyModule
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };

        public TripModule() : base("/api/v1")
        {
            Get["/trips/{tripId:int}"] = GetTrip;
            Get["/trips"] = GetTripsQueryResponse;
            Delete["/trips"] = ClearTrips;
        }

        private static Response GetTrip(dynamic parameters)
        {
            Response response;
            var trip = TripBl.GetTripWithMessages((int)parameters.tripId);
            if (null == trip)
            {
                response = string.Empty;
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response = JsonConvert.SerializeObject(trip);
                response.ContentType = "application/json";
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }

        private Response GetTripsQueryResponse(dynamic parameters)
        {
            Response response;
            try
            {
                response = GetTripsQueryResponseString();
                response.ContentType = "application/json";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response = GetErrorMessage(ex);
                response.ContentType = "application/json";
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        private string GetTripsQueryResponseString()
        {
            string responseString = null;
            switch (GetQueryType())
            {
                case QueryType.PointInTime:
                    responseString = GetTotalTripsOccuringAtTimeResponseString();
                    break;
                case QueryType.StartStopInGeoRect:
                    responseString = GetTotalTripsAndFaresThatStartOrStopInGeoRectangle();
                    break;
                case QueryType.ThroughGeoRect:
                    responseString = GetTotalTripsThroughGeoRectangleResponseString();
                    break;
            }
            return responseString;
        }

        private QueryType GetQueryType()
        {
            try
            {
                return TryGetQueryType();
            }
            catch (Exception)
            {
                throw new QueryException(
                    @"type is a required field and must be either ""ThroughGeoRect"", ""StartStopInGeoRect"", or ""PointInTime"".");
            }
        }

        private QueryType TryGetQueryType()
        {
            var queryType = (string)Request.Query["type"];
            if (null == queryType)
                throw new Exception();
            return (QueryType)Enum.Parse(typeof(QueryType), queryType);
        }

        private string GetTotalTripsOccuringAtTimeResponseString()
        {
            var epoch = GetEpoch();
            return
                JsonConvert.SerializeObject(
                    new
                    {
                        queryType = QueryType.PointInTime,
                        epoch,
                        numberOfTrips = TripBl.GetTotalTripsOccuringAtTime(epoch)
                    }, SerializerSettings);
        }

        private long GetEpoch()
        {
            try
            {
                return TryGetEpoch();
            }
            catch (Exception)
            {
                throw new QueryException("epoch is a required field for this query and must be a positive integer.");
            }
        }

        private long TryGetEpoch()
        {
            var epoch = (string)Request.Query["epoch"];
            if (null == epoch)
                throw new Exception();
            return long.Parse(epoch);
        }

        private string GetTotalTripsAndFaresThatStartOrStopInGeoRectangle()
        {
            var geoRectangle = GetGeoRectangle();
            var totalTripsAndFares = TripBl.GetTotalTripsAndFaresThatStartOrStopInGeoRectangle(geoRectangle);
            return
                JsonConvert.SerializeObject(
                    new
                    {
                        queryType = QueryType.StartStopInGeoRect,
                        geoRectangle,
                        totalTrips = (int)totalTripsAndFares.TotalTrips,
                        totalFares = (decimal)totalTripsAndFares.TotalFares
                    }, SerializerSettings);
        }

        private GeoRectangle GetGeoRectangle()
        {
            return new GeoRectangle(new GeoPoint(GetDecimal("bottomLeftLatitude"), GetDecimal("bottomLeftLongitude")),
                new GeoPoint(GetDecimal("topRightLatitude"), GetDecimal("topRightLongitude")));
        }

        private decimal GetDecimal(string name)
        {
            try
            {
                return TryGetDecimal(name);
            }
            catch (Exception)
            {
                throw new QueryException("{0} is a required field for this query and must be a decimal.", name);
            }
        }

        private decimal TryGetDecimal(string name)
        {
            var deccimalString = (string)Request.Query[name];
            if (null == deccimalString)
                throw new Exception();
            return decimal.Parse(deccimalString);
        }

        private string GetTotalTripsThroughGeoRectangleResponseString()
        {
            var geoRectangle = GetGeoRectangle();
            return
                JsonConvert.SerializeObject(
                    new
                    {
                        queryType = QueryType.ThroughGeoRect,
                        geoRectangle,
                        totalTrips = TripBl.GetTotalTripsThroughGeoRectangle(geoRectangle)
                    }, SerializerSettings);
        }

        private static string GetErrorMessage(Exception ex)
        {
            return JsonConvert.SerializeObject(new { error = ex.Message, errorType = ex.GetType().ToString() });
        }

        private static Response ClearTrips(dynamic parameters)
        {
            TripBl.TruncateTrips();
            return new Response { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
