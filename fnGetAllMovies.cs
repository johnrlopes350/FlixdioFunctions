 using System.Collections.Generic;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Azure.WebJobs;
  using Microsoft.Azure.WebJobs.Extensions.Http;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;
  using Azure.Cosmos;
  using System.Threading.Tasks;

  public static class fnGetAllMovies
  {
      [FunctionName("fnGetAllMovies")]
      public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
          ILogger log)
      {
          log.LogInformation("Recebendo requisição para listar todos os filmes.");

          var cosmosClient = new CosmosClient("<sua-connection-string>");
          var container = cosmosClient.GetContainer("flixdio", "filmes");

          var query = "SELECT * FROM c";
          var iterator = container.GetItemQueryIterator<dynamic>(query);
          var results = new List<dynamic>();

          while (iterator.HasMoreResults)
          {
              foreach (var item in await iterator.ReadNextAsync())
              {
                  results.Add(item);
              }
          }

          return new OkObjectResult(results);
      }
  }
