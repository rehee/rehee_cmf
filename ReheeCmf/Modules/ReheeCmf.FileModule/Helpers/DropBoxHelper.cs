using Dropbox.Api;
using Dropbox.Api.Files;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.FileServices;
using ReheeCmf.Helpers;
using ReheeCmf.Responses;

namespace ReheeCmf.FileModule.Helpers
{
  public static class DropBoxHelper
  {
    public static async Task<ContentResponse<FileServiceResponse>> DropBoxUpload(
      this FileServiceRequest request, FileServiceOption option, CancellationToken? ct = null)
    {
      var result = new ContentResponse<FileServiceResponse>();
      var dbx = new DropboxClient(option.AccessToken);
      try
      {
        var uuid = request.FileUUID ?? $"{Guid.NewGuid().ToString()}{request.GetFileExtension()}";
        var filePath = $@"/{uuid}";
        FileMetadata response = null;
        if (ct == null)
        {
          response = await dbx.Files.UploadAsync(filePath, body: request.Content);
        }
        else
        {
          var task = Task<FileMetadata>.Run(async () => await dbx.Files.UploadAsync(filePath, body: request.Content), ct.Value);
          do
          {
            await Task.Delay(1000);
            if (ct.Value.IsCancellationRequested)
            {
              request.Content.Dispose();
              task.Dispose();
              break;
            }
          } while (!task.IsCompleted);
          response = task.IsCompleted ? task.Result : null;
        }
        if (ct.HasValue && ct.Value.IsCancellationRequested)
        {
          result.Cancel = true;
          return result;
        }
        result.SetSuccess(new FileServiceResponse()
        {
          FileUUID = uuid
        });
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.ErrorMessage = ex.Message;
      }
      return result;
    }
    public static async Task<ContentResponse<FileServiceResponse>> DropBoxDownload(
      this FileServiceRequest request, FileServiceOption option, CancellationToken? ct = null)
    {
      var result = new ContentResponse<FileServiceResponse>();
      var dbx = new DropboxClient(option.AccessToken);
      try
      {
        var response = await dbx.Files.DownloadAsync($"/{request.FileUUID}");
        var ms = new MemoryStream();
        await (await response.GetContentAsStreamAsync()).CopyToAsync(ms);
        ms.Position = 0;
        var fileContent = new FileServiceResponse()
        {
          Content = ms,
          FileUUID = request.FileUUID,
          FileName = response.Response.AsFile.Name,
          ContentType = request.GetFileContentType(false)
        };
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }
    public static async Task<ContentResponse<FileServiceResponse>> DropBoxDelete(
      this FileServiceRequest request, FileServiceOption option, CancellationToken? ct = null)
    {
      var result = new ContentResponse<FileServiceResponse>();
      var dbx = new DropboxClient(option.AccessToken);
      try
      {
        var response = await dbx.Files.DeleteV2Async($"/{request.FileUUID}");
        result.SetSuccess(null);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.ErrorMessage = ex.Message;
      }
      return result;
    }
    public static async Task<ContentResponse<string>> DropBoxOutLink(
      this string uuid, FileServiceOption option, CancellationToken? ct = null)
    {
      var result = new ContentResponse<string>();
      var dbx = new DropboxClient(option.AccessToken);
      try
      {
        var sl = await dbx.Sharing.ListSharedLinksAsync($"/{uuid}", null, true);
        if (sl.Links.Any())
        {
          result.SetSuccess(
            sl.Links.FirstOrDefault().Url
            .Replace("www.dropbox.com", "dl.dropboxusercontent.com"));
          return result;
        }

      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      try
      {
        var sl2 = await dbx.Sharing.CreateSharedLinkWithSettingsAsync($"/{uuid}");
        result.SetSuccess(
            sl2.Url
            .Replace("www.dropbox.com", "dl.dropboxusercontent.com"));
        return result;
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }
  }
}
