using ReheeCmf.Commons.DTOs;
using ReheeCmf.FileModule.Helpers;
using ReheeCmf.FileServices;
using ReheeCmf.Responses;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.FileModule.Services.Implements
{
  public class DropboxImplement : IFileService
  {
    private readonly FileServiceOption options;
    private readonly IHttpClientFactory clientFactory;

    public DropboxImplement(FileServiceOption options, IHttpClientFactory clientFactory)
    {
      this.options = options;
      this.clientFactory = clientFactory;
    }
    public bool RedirectUrl { get; } = true;
    public async Task<ContentResponse<FileServiceResponse>> DeleteFile(FileServiceRequest request, CancellationToken? ct)
    {
      return await DropBoxHelper.DropBoxDelete(request, options);
    }

    public async Task<ContentResponse<FileServiceResponse>> DownloadFile(FileServiceRequest request, CancellationToken? ct)
    {
      return await DropBoxHelper.DropBoxDownload(request, options);
    }

    public async Task<ContentResponse<string>> GetUrl(FileServiceRequest request, CancellationToken? ct)
    {
      return await DropBoxHelper.DropBoxOutLink(request.FileUUID, options);
    }

    public async Task<ContentResponse<FileServiceResponse>> UploadFile(FileServiceRequest request, CancellationToken? ct)
    {
      return await DropBoxHelper.DropBoxUpload(request, options, ct);
    }
  }
}
