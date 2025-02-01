using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Blob;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

public static class UploadImageFunction
{
    [FunctionName("UploadImage")]
    public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        [Blob("images/{rand-guid}.jpg", FileAccess.Write)] CloudBlockBlob blob,
        ILogger log)
    {
        var formdata = await req.ReadFormAsync();
        var file = formdata.Files["file"];

        if (file == null || !file.ContentType.StartsWith("image/"))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Invalid file type. Only image files are allowed.")
            };
        }

        using (var stream = file.OpenReadStream())
        {
            await blob.UploadFromStreamAsync(stream);
        }

        log.LogInformation($"File uploaded: {file.FileName} to Blob Storage.");

        // Optionally trigger background processing (e.g., resizing, watermarking)
        // Enqueue message to queue for async processing
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"File uploaded successfully! URL: {blob.Uri}")
        };
    }
}
