using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

public class InvoiceFunction
{
    private readonly ILogger<InvoiceFunction> _logger;
    private readonly ECManager _ecManager;
    private readonly EE1Manager _ee1Manager;
    private readonly EE2Manager _ee2Manager;
    private readonly EnergiaActivaManager _energiaActivaManager;

    public InvoiceFunction(ILogger<InvoiceFunction> logger, ECManager ecManager, EE1Manager ee1Manager, EE2Manager ee2Manager, EnergiaActivaManager energiaActivaManager)
    {
        _logger = logger;
        _ecManager = ecManager;
        _ee1Manager = ee1Manager;
        _ee2Manager = ee2Manager;
        _energiaActivaManager = energiaActivaManager;
    }

    [Function("CalculateInvoice")]
    [OpenApiOperation(operationId: "CalculateInvoice", tags: ["Invoice"], Description = "Calculates the invoice for a client and a specific month.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(object), Required = true, Description = "Invoice calculation parameters.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Invoice calculated successfully.")]
    public async Task<IActionResult> CalculateInvoice([HttpTrigger(AuthorizationLevel.Function, "post", Route = "calculate-invoice")] HttpRequestData req)
    {
        _logger.LogInformation("Calculating invoice...");
        return new OkObjectResult(new { Message = "Invoice calculated successfully" });
    }

    [Function("ClientStatistics")]
    [OpenApiOperation(operationId: "ClientStatistics", tags: ["Statistics"], Description = "Provides consumption and injection statistics for a client.")]
    [OpenApiParameter(name: "client_id", In = ParameterLocation.Path, Required = true, Description = "Client identifier.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Client statistics retrieved successfully.")]
    public IActionResult GetClientStatistics([HttpTrigger(AuthorizationLevel.Function, "get", Route = "client-statistics/{client_id}")] HttpRequestData req, string client_id)
    {
        _logger.LogInformation($"Retrieving statistics for client {client_id}...");
        return new OkObjectResult(new { ClientId = client_id, Message = "Statistics retrieved successfully" });
    }

    [Function("SystemLoad")]
    [OpenApiOperation(operationId: "SystemLoad", tags: ["Load"], Description = "Displays system load per hour based on consumption data.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "System load retrieved successfully.")]
    public IActionResult GetSystemLoad([HttpTrigger(AuthorizationLevel.Function, "get", Route = "system-load")] HttpRequestData req)
    {
        _logger.LogInformation("Retrieving system load...");
        return new OkObjectResult(new { Message = "System load retrieved successfully" });
    }

    [Function("CalculateConcept")]
    [OpenApiOperation(operationId: "CalculateConcept", tags: ["Concept"], Description = "Calculates individual energy concepts.")]
    [OpenApiParameter(name: "concept", In = ParameterLocation.Query, Required = true, Description = "Energy concept to calculate (EC, EE1, EE2, EA).")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Concept calculated successfully.")]
    public async Task<IActionResult> CalculateConcept([HttpTrigger(AuthorizationLevel.Function, "get", Route = "calculate-concept")] HttpRequestData req)
    {
        var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string concept = queryParams["concept"];
        object result = null;

        switch (concept?.ToUpper())
        {
            case "EC":
                result = _ecManager.CalcularEC();
                break;
            case "EE1":
                result = _ee1Manager.CalcularEE1();
                break;
            case "EE2":
                result = await _ee2Manager.CalculateEE2Async();
                break;
            case "EA":
                result = _energiaActivaManager.CalcularEnergiaActiva();
                break;
            default:
                return new BadRequestObjectResult("Invalid concept parameter.");
        }
        return new OkObjectResult(result);
    }
}

