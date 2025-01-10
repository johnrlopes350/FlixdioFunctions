 using System.IO;
  using System.Text.Json;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Azure.WebJobs;
  using Microsoft.Azure.WebJobs.Extensions.Http;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;
  using Azure.Cosmos;

  public static class fnPostDatabase
  {
      [FunctionName("fnPostDatabase")]
      public static IActionResult Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
          ILogger log)
      {
          log.LogInformation("Recebendo requisição para salvar no CosmosDB.");

          var cosmosClient = new CosmosClient("<sua-connection-string>");
          var container = cosmosClient.GetContainer("flixdio", "filmes");

          using (var reader = new StreamReader(req.Body))
          {
              var content = reader.ReadToEnd();
              var document = JsonSerializer.Deserialize<dynamic>(content);
              container.CreateItemAsync(document);
          }

          return new OkObjectResult("Item salvo com sucesso no CosmosDB.");
      }
  }