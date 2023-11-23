using Azure.Storage.Blobs;

namespace Delivery3Admin.Services;

public interface IStorageBlob
{
    Task<string> UploadFileAsync(string absoluteFilePath, Stream stream);
}

public class StorageBlob : IStorageBlob
{
    private readonly BlobContainerClient _containerClient;
    private const string ContainerImages = "images";
    
    public StorageBlob(string connStr)
    {
        var blobServiceClient = new BlobServiceClient(connStr);
        _containerClient = blobServiceClient.GetBlobContainerClient(ContainerImages);
        _containerClient.CreateIfNotExists();
    }
    
    public async Task<string> UploadFileAsync(string absoluteFilePath, Stream stream)
    {
        BlobClient? blobClient = _containerClient.GetBlobClient(absoluteFilePath);
        _ = await blobClient.UploadAsync(stream, true);
        
        return blobClient.Uri.AbsoluteUri;
    }
}