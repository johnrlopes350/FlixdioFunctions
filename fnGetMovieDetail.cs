  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Azure.WebJobs;
  using Microsoft.Azure.WebJobs.Extensions.Http;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;
  using Azure.Cosmos;
  using System.Threading.Tasks;

  public static class fnGetMovieDetail
  {
      [FunctionName("fnGetMovieDetail")]
      public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id}")] HttpRequest req,
          string id,
          ILogger log)
      {
          log.LogInformation($"Recebendo requisição para obter detalhes do filme com ID: {id}");

          var cosmosClient = new CosmosClient("<sua-connection-string>");
          var container = cosmosClient.GetContainer("flixdio", "filmes");

          var response = await container.ReadItemAsync<dynamic>(id, new PartitionKey(id));
          return new OkObjectResult(response.Resource);
      }
  }
