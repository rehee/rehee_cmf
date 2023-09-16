using ReheeCmf.Commons.DTOs;
using ReheeCmf.FileModule.Helpers;
using ReheeCmf.FileServices;
using ReheeCmf.Helpers;
using ReheeCmf.Responses;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.FileModule.Services.Implements
{
  public class LocalImplement : IFileService
  {
    private readonly FileServiceOption options;

    public LocalImplement(FileServiceOption options)
    {
      this.options = options;

    }
    public bool RedirectUrl { get; } = false;
    public async Task<ContentResponse<FileServiceResponse>> DeleteFile(FileServiceRequest request, CancellationToken? ct)
    {
      return await LocalHelper.LocalDeleteFile(request, options, ct ?? CancellationToken.None);
    }

    public async Task<ContentResponse<FileServiceResponse>> DownloadFile(FileServiceRequest request, CancellationToken? ct)
    {
      return await LocalHelper.LocalDownloaddFile(request, options, ct ?? CancellationToken.None);
    }

    public Task<ContentResponse<string>> GetUrl(FileServiceRequest request, CancellationToken? ct)
    {
      var result = new ContentResponse<string>();
      var url = $"/{FileServiceOption.FileApi}/uuid/{request.FileUUID}";
      result.SetSuccess(url);
      return Task.FromResult(result);
    }

    public async Task<ContentResponse<FileServiceResponse>> UploadFile(FileServiceRequest request, CancellationToken? ct)
    {
      return await LocalHelper.LocalUploadFile(request, options, ct ?? CancellationToken.None);
    }
  }
}
