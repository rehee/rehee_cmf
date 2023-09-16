using ReheeCmf.Commons.DTOs;
using ReheeCmf.FileServices;
using ReheeCmf.Helpers;
using ReheeCmf.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.FileModule.Helpers
{
  public static class LocalHelper
  {
    public static async Task<ContentResponse<FileServiceResponse>> LocalUploadFile(
      FileServiceRequest request, FileServiceOption options, CancellationToken ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      if (String.IsNullOrEmpty(request.FileUUID))
      {
        request.FileUUID = $"{Guid.NewGuid().ToString()}{request.GetFileExtension()}";
      }
      var folder = request.GetFilePath(false, options);
      if (!System.IO.Directory.Exists(folder))
      {
        System.IO.Directory.CreateDirectory(folder);
      }
      try
      {
        var file = request.GetFilePath(true, options);
        if (System.IO.File.Exists(file))
        {
          File.Delete(file);
        }
        var fileStream = File.Create($@"{folder}\{request.FileUUID}");
        if (request.Content.Position > 0)
        {
          try
          {
            request.Content.Seek(0, SeekOrigin.Begin);
          }
          catch (Exception ex)
          {
            ex.ThrowStatusException();
          }
        }

        await request.Content.CopyToAsync(fileStream);
        fileStream.Close();
        result.SetSuccess(new FileServiceResponse()
        {
          FileUUID = request.FileUUID
        });
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }

    public static Task<ContentResponse<FileServiceResponse>> LocalDownloaddFile(
      FileServiceRequest request, FileServiceOption options, CancellationToken ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      if (options.AuthRequired && !request.Auth)
      {
        result.Status = HttpStatusCode.Unauthorized;
        goto goResult;
      }
      var file = request.GetFilePath(true, options);
      if (!File.Exists(file))
      {
        goto goResult;
      }
      try
      {
        var fileStream = File.OpenRead(file);
        result.SetSuccess(new FileServiceResponse()
        {
          Content = fileStream,
          FileUUID = request.FileUUID,
          ContentType = request.GetFileContentType(false)
        });
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();

      }
    goResult:
      return Task.FromResult(result);
    }
    public static Task<ContentResponse<FileServiceResponse>> LocalDeleteFile(
      FileServiceRequest request, FileServiceOption options, CancellationToken ct)
    {
      var result = new ContentResponse<FileServiceResponse>();
      var file = request.GetFilePath(true, options);
      if (!System.IO.File.Exists(file))
      {
        goto toResult;
      }
      try
      {
        File.Delete(file);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();

      }
    toResult:
      return Task.FromResult(result);
    }
  }
}
