using Nancy;

namespace GeoTemporalApi
{
    public class HealthCheckModule : NancyModule
    {
        public HealthCheckModule()
        {
            Get["healthCheck"] = GetHealthCheckResponse;
        }

        private static Response GetHealthCheckResponse(dynamic parameters)
        {
            Response response = "Health Check Passed";
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
