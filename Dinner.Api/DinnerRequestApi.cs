using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dinner.Library;
using Newtonsoft.Json.Linq;

namespace Dinner.Api
{
    public class DinnerRequestApi
    {
        private readonly ILogger<DinnerRequestApi> _logger;

        public DinnerRequestApi(ILogger<DinnerRequestApi> logger)
        {
            _logger = logger;
        }

       
        [Function("GetDinnerRequests")]
        public async Task<IActionResult> GetDinnerRequests([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Return a list of dinner requests");
            var dinnerRequests = await DinnerRequestHelper.GetDinnerRequestsAsync();
            return new OkObjectResult(dinnerRequests);
        }

        [Function("GetDinnerRequest")]
        public async Task<IActionResult> GetDinnerRequest([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var id = req.Query["dinnerRequestId"];
            _logger.LogInformation("Return a dinnerRequest");
            var dinnerRequest = await DinnerRequestHelper.GetDinnerRequestAsync(id);
            return new OkObjectResult(dinnerRequest);
        }

        [Function("CreateDinnerRequest")]
        public async Task<IActionResult> CreateDinnerRequest([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string dinnerRequestJson = await new StreamReader(req.Body).ReadToEndAsync();
            var dinnerRequestObject = JObject.Parse(dinnerRequestJson);

            Console.WriteLine("Dinner Request:" + dinnerRequestObject);

            DinnerRequest dinnerRequest  = new DinnerRequest((string)dinnerRequestObject["name"],(string)dinnerRequestObject["email"],(string)dinnerRequestObject["restaurant"],(DateTime)dinnerRequestObject["time"]);
            await DinnerRequestHelper.CreateDinnerRequestAsync(dinnerRequest);

            _logger.LogInformation("Create a new dinner Request");

            return new OkObjectResult(true);
        }
        [Function("UpdateDinnerRequest")]
        public async Task<IActionResult> UpdateDinnerRequest([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string dinnerRequestJson = await new StreamReader(req.Body).ReadToEndAsync();
            var dinnerRequestObject = JObject.Parse(dinnerRequestJson);

            if (string.IsNullOrEmpty((string)dinnerRequestObject["id"]))
            {
                throw new Exception("Unable to update user without ID");
            }

            DinnerRequest dinnerRequest  = new DinnerRequest((string)dinnerRequestObject["name"], (string)dinnerRequestObject["email"], (string)dinnerRequestObject["restaurant"], (DateTime)dinnerRequestObject["time"]);
            dinnerRequest.Id = Guid.Parse((string)dinnerRequestObject["id"]);

            await DinnerRequestHelper.UpdateDinnerRequestAsync(dinnerRequest);
            _logger.LogInformation("Updated dinner request with ID: " + dinnerRequest.Id);
            return new OkObjectResult(true);
        }

    }
}
