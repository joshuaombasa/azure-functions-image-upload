public class UploadImageFunctionTests
{
    [Fact]
    public async Task UploadImage_ReturnsError_WhenFileIsNotImage()
    {
        // Arrange
        var req = CreateHttpRequestWithInvalidFile();
        var blobMock = new Mock<CloudBlockBlob>(new Uri("http://localhost"));

        // Act
        var response = await UploadImageFunction.Run(req, blobMock.Object, NullLogger.Instance);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
