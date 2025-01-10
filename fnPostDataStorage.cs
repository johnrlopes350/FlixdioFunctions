using System.IO;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Azure.WebJobs;
  using Microsoft.Azure.WebJobs.Extensions.Http;
  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Logging;
  using Azure.Storage.Blobs;

  public static class fnPostDataStorage
  {
      [FunctionName("fnPostDataStorage")]
      public static IActionResult Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
          ILogger log)
      {
          log.LogInformation("Recebendo requisição para upload de arquivo.");

          var blobServiceClient = new BlobServiceClient("<sua-connection-string>");
          var containerClient = blobServiceClient.GetBlobContainerClient("filmes");
          containerClient.CreateIfNotExists();

          var file = req.Form.Files[0];
          using (var stream = file.OpenReadStream())
          {
              var blobClient = containerClient.GetBlobClient(file.FileName);
              blobClient.Upload(stream, overwrite: true);
          }

          return new OkObjectResult("Arquivo enviado com sucesso.");
      }
  }
