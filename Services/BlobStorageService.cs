using Azure.Storage.Blobs;

public class BlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(string connectionString, string containerName)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task UploadBlobAsync(string blobName, Stream data)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(data, overwrite: true);
    }

    public string GetBlobUrl(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        return blobClient.Uri.ToString();
    }
}
