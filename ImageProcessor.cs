public static class ImageProcessor
{
    [FunctionName("ProcessImage")]
    public static async Task Run(
        [QueueTrigger("image-processing-queue", Connection = "AzureWebJobsStorage")] string imageBlobUrl,
        [Blob("images-processed/{rand-guid}.jpg", FileAccess.Write)] CloudBlockBlob outputBlob,
        ILogger log)
    {
        log.LogInformation($"Processing image: {imageBlobUrl}");

        // Download and process image (resize, watermark)
        // For simplicity, assume we transform the image here

        using (var outputStream = new MemoryStream())
        {
            // Apply transformations (resize, watermark, etc.) here
            await outputBlob.UploadFromStreamAsync(outputStream);
        }

        log.LogInformation($"Image processed and saved to: {outputBlob.Uri}");
    }
}
