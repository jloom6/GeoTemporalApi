using GeoTemporalBl;
using GeoTemporalModels;
using Nancy;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GeoTemporalApi
{
    public class TripMessageModule : NancyModule
    {
        public TripMessageModule() : base("/api/v1")
        {
            Post["/tripMessages"] = AddTripMessage;
        }

        private Response AddTripMessage(dynamic parameters)
        {
            Response response;
            try
            {
                TripMessageBl.InsertTripMessage(GetTripMessage());
                response = new Response {StatusCode = HttpStatusCode.Created};
            }
            catch (Exception ex)
            {
                response = GetErrorMessage(ex);
                response.ContentType = "application/json";
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        private TripMessage GetTripMessage()
        {
            using (Request.Body)
            using (var streamReader = new StreamReader(Request.Body))
                return JsonConvert.DeserializeObject<TripMessage>(streamReader.ReadToEnd());
        }

        private static string GetErrorMessage(Exception ex)
        {
            return JsonConvert.SerializeObject(new {error = ex.Message, errorType = ex.GetType().ToString()});
        }
    }
}
