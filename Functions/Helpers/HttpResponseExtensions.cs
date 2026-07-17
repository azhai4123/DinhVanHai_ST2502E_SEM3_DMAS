using Microsoft.Azure.Functions.Worker.Http;

namespace Functions.Helpers
{
    public static class HttpResponseExtensions
    {
        public static HttpResponseData AddCors(this HttpResponseData response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
            return response;
        }
    }
}
