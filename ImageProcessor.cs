using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

public static class ImageProcessor
{
    private static readonly HttpClient HttpClient = new HttpClient();

    [FunctionName("ProcessImage")]
    public static async Task Run(
        [QueueTrigger("image-processing-queue", Connection = "AzureWebJobsStorage")] string imageBlobUrl,
        [Blob("images-processed/{rand-guid}.jpg", FileAccess.Write)] CloudBlockBlob outputBlob,
        ILogger log)
    {
        log.LogInformation($"Processing image: {imageBlobUrl}");

        try
        {
            using (var response = await HttpClient.GetAsync(imageBlobUrl))
            {
                if (!response.IsSuccessStatusCode)
                {
                    log.LogError($"Failed to download image. Status code: {response.StatusCode}");
                    return;
                }

                using (var inputStream = await response.Content.ReadAsStreamAsync())
                using (var outputStream = new MemoryStream())
                {
                    // Apply transformations (resize, watermark, etc.)
                    await ProcessImageAsync(inputStream, outputStream);
                    outputStream.Seek(0, SeekOrigin.Begin);

                    await outputBlob.UploadFromStreamAsync(outputStream);
                }
            }
            log.LogInformation($"Image successfully processed and saved to: {outputBlob.Uri}");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error processing image");
        }
    }

    private static async Task ProcessImageAsync(Stream input, Stream output)
    {
        // Placeholder for image processing logic (resize, watermark, etc.)
        await input.CopyToAsync(output);
    }
}
