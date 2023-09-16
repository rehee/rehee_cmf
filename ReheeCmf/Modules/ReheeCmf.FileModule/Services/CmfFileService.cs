using ReheeCmf.Commons.DTOs;
using ReheeCmf.Commons;
using ReheeCmf.Enums;
using ReheeCmf.FileServices;
using ReheeCmf.MultiTenants;
using ReheeCmf.Services;
using ReheeCmf.Tenants;
using ReheeCmf.Responses;
using ReheeCmf.Helpers;
using ReheeCmf.FileModule.Services.Implements;

namespace ReheeCmf.FileModule.Services
{
  public class CmfFileService : ServiceWithTenant, IFileService
  {
    private readonly FileServiceOption options;
    private readonly IHttpClientFactory clientFactory;
    private IFileService service
    {
      get
      {
        if (_service == null)
        {
          _service = GetService();
        }
        return _service;
      }
    }
    private IFileService GetService()
    {
      var optionCheck = CurrentTenant?.FileOption ?? this.options;
      AlowedFileTypes = String.IsNullOrEmpty(optionCheck?.AllowedFileType) ?
      Enumerable.Empty<string>().ToArray()
      : optionCheck?.AllowedFileType.Split(",").Select(b => b.Trim()).ToArray();

      switch (options.ServiceType)
      {
        case EnumFileService.Dropbox:
          return new DropboxImplement(optionCheck, clientFactory);
        case EnumFileService.Local:
          return new LocalImplement(optionCheck);
        case EnumFileService.AzureBlob:
          return new AZBlobImplement(optionCheck);
        default:
          return null;
      }
    }
    private IFileService _service { get; set; } = null;

    public bool RedirectUrl => service?.RedirectUrl ?? false;
    public string[] AlowedFileTypes;

    public CmfFileService(FileServiceOption options, IHttpClientFactory clientFactory, IContextScope<Tenant> tenantDetail) : base(tenantDetail)
    {
      this.options = options;
      this.clientFactory = clientFactory;
      TenantChange += (a, b) =>
      {
        _service = null;
      };
    }
    public async Task<ContentResponse<FileServiceResponse>> DeleteFile(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      if (service != null)
      {
        return await service.DeleteFile(request, ct);
      }
      return result;
    }
    public async Task<ContentResponse<FileServiceResponse>> DownloadFile(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      if (service != null)
      {
        return await service.DownloadFile(request, ct);
      }
      return result;
    }
    public async Task<ContentResponse<FileServiceResponse>> UploadFile(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      var validateFile = options.ValidateFile(request.FileName, request.Content.Length);
      if (!validateFile.Success)
      {
        result.SetError(validateFile);
        return result;
      }
      request.FileUUID = request.FileUUID ?? $"{Guid.NewGuid().ToString()}{request.GetFileExtension()}";
      if (service != null)
      {
        return await service.UploadFile(request, ct);
      }
      return result;
    }
    public async Task<ContentResponse<string>> GetUrl(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<string>();
      if (service != null)
      {
        return await service.GetUrl(request, ct);
      }
      return result;
    }


  }
}
