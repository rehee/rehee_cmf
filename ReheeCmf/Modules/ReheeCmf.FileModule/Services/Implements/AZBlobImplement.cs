using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.FileServices;
using ReheeCmf.Services;
using System.Net;
using ReheeCmf.Responses;
using ReheeCmf.Helpers;

namespace ReheeCmf.FileModule.Services.Implements
{
  public class AZBlobImplement : IFileService
  {
    private readonly FileServiceOption options;
    private BlobContainerClient containerClient;
    private BlobContainerClient containerCompressionClient;
    private string[] CompressionFileExtensions { get; set; }
    public AZBlobImplement(FileServiceOption options)
    {
      this.options = options;
      if (!String.IsNullOrEmpty(options.CompressionFileExtensions))
      {
        CompressionFileExtensions = options.CompressionFileExtensions.Split(",").Select(b => $".{b.Trim()}").ToArray();
      }
      else
      {
        CompressionFileExtensions = new string[] { ".jpg", ".png", ".bmp" };
      }
      var connectionString = this.options.AccessToken;
      var containerName = this.options.BaseFolder ?? FileServiceOption.DefaultBaseFolder;
      var blobServiceClient = new BlobServiceClient(connectionString);

      containerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower());
      var authLevel = options.AuthRequired ? PublicAccessType.None : PublicAccessType.Blob;
      containerClient.CreateIfNotExists();
      containerClient.SetAccessPolicy(authLevel);
      containerCompressionClient = blobServiceClient.GetBlobContainerClient($"{containerName.ToLower()}compression");
      containerCompressionClient.CreateIfNotExists();
      containerCompressionClient.SetAccessPolicy(authLevel);

    }
    public bool RedirectUrl => true;

    public async Task<ContentResponse<FileServiceResponse>> DeleteFile(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      try
      {
        var blobClient = containerClient.GetBlobClient(request.FileUUID);
        await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
        if (this.options.Compression)
        {
          await containerCompressionClient.GetBlobClient(request.FileUUID).DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
        }
        result.SetSuccess(new FileServiceResponse
        {
          FileUUID = request.FileUUID
        });
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.Status = System.Net.HttpStatusCode.InternalServerError;
        result.ErrorMessage = ex.Message;
      }

      return result;
    }

    public async Task<ContentResponse<FileServiceResponse>> DownloadFile(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      if (options.AuthRequired && !request.Auth)
      {
        result.Status = HttpStatusCode.Unauthorized;
        return result;
      }
      try
      {
        var blobClient = containerClient.GetBlobClient(request.FileUUID);
        if (blobClient == null || !blobClient.Exists())
        {
          result.Status = HttpStatusCode.NotFound;
          return result;
        }
        var stream = new MemoryStream();
        var downloadResponse = await blobClient.DownloadToAsync(stream);

        if (downloadResponse.IsError)
        {
          result.Status = System.Net.HttpStatusCode.InternalServerError;
          return result;
        }
        stream.Seek(0, SeekOrigin.Begin);
        result.SetSuccess(new FileServiceResponse
        {
          FileUUID = request.FileUUID,
          Content = stream,
          ContentType = request.GetFileContentType(false)
        }); ;
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.Status = System.Net.HttpStatusCode.InternalServerError;
        result.ErrorMessage = ex.Message;
      }

      return result;
    }

    public Task<ContentResponse<string>> GetUrl(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<string>();
      try
      {
        var blobClient = containerClient.GetBlobClient(request.FileUUID);
        if (blobClient != null)
        {
          result.SetSuccess((request.Auth && options.AuthRequired) ?
            blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.Now.AddMinutes(30)).AbsoluteUri :
            blobClient.Uri.AbsoluteUri);
        }
        var compressClient = containerCompressionClient.GetBlobClient(request.FileUUID);
        if (this.options.Compression && compressClient != null && compressClient.Exists())
        {
          result.SetSuccess(
            (request.Auth && options.AuthRequired) ?
            compressClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.Now.AddMinutes(30)).AbsoluteUri :
            compressClient.Uri.AbsoluteUri);
        }
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.Status = System.Net.HttpStatusCode.InternalServerError;
        result.ErrorMessage = ex.Message;
      }

      return Task.FromResult(result);
    }

    public async Task<ContentResponse<FileServiceResponse>> UploadFile(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      try
      {
        var blobClient = this.options.Compression ? CompressionFileExtensions.Any(b => b.Equals(request.GetFileExtension(), StringComparison.OrdinalIgnoreCase)) ? containerCompressionClient.GetBlobClient(request.FileUUID) :
          containerClient.GetBlobClient(request.FileUUID) : containerClient.GetBlobClient(request.FileUUID);
        await blobClient.UploadAsync(request.Content);
        result.SetSuccess(new FileServiceResponse
        {
          FileUUID = request.FileUUID
        });
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.Status = System.Net.HttpStatusCode.InternalServerError;
        result.ErrorMessage = ex.Message;
      }

      return result;
    }
  }
}
