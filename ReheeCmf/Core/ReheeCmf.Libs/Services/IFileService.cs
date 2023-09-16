using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IFileService
  {
    bool RedirectUrl { get; }
    Task<ContentResponse<FileServiceResponse>> UploadFile(FileServiceRequest request, CancellationToken? ct);
    Task<ContentResponse<FileServiceResponse>> DownloadFile(FileServiceRequest request, CancellationToken? ct);
    Task<ContentResponse<FileServiceResponse>> DeleteFile(FileServiceRequest request, CancellationToken? ct);
    Task<ContentResponse<string>> GetUrl(FileServiceRequest request, CancellationToken? ct);
  }
}
