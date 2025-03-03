using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace FoxTrack
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        [OpenApiOperation(operationId: nameof(Run), tags: [nameof(Function1)], Description = "Save or update an aseguramiento record.")]
        [OpenApiParameter(name: "oid", In = ParameterLocation.Header, Required = true, Description = "The oid of the user.")]
        [OpenApiParameter(name: "idFinca", In = ParameterLocation.Header, Required = true, Description = "The property idFinca.")]
        [OpenApiParameter(name: "idBloque", In = ParameterLocation.Header, Required = true, Description = "The property ID Bloque.")]
        [OpenApiParameter(name: "idCalendar", In = ParameterLocation.Header, Required = true, Description = "The property ID Calendar.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(int), Required = true, Description = "Aseguramiento details.")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(bool), Description = "Aseguramiento record saved or updated successfully.")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Invalid request.")]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(string), Description = "Internal server error.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.Unauthorized, Description = "Unauthorized access.")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
