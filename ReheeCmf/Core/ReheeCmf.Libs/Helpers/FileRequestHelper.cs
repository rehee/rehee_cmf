using ReheeCmf.Commons.Interfaces;
using ReheeCmf.Commons;
namespace ReheeCmf.Helpers
{
  public static class FileRequestHelper
  {
    public static string GetFilePath(this IFileInfo info, bool includeFile = true, FileServiceOption option = null)
    {
      var filePaths = new List<string>();
      if (option != null)
      {
        switch (option.ServiceType)
        {
          case EnumFileService.Dropbox:
            break;
          default:
            filePaths.Add(option.ServerPath ?? FileServiceOption.DefaultServerPath);
            filePaths.Add(option.BaseFolder ?? FileServiceOption.DefaultBaseFolder);
            break;
        }
      }
      if (!String.IsNullOrEmpty(info.Path))
      {
        foreach (var p in info.Path.Split("/"))
        {
          filePaths.Add(p);
        }
      }
      if (includeFile)
      {
        filePaths.Add(info.FileUUID);
      }
      return string.Join("/", filePaths);
    }
    public static string GetFileExtension(this string fileName)
    {
      return "." + fileName.Split(".").LastOrDefault();
    }

    public static string GetFileExtension(this FileServiceRequest request)
    {
      var extensions = Common.ContentExtensionMap.FirstOrDefault(b => b.ContentType == request.ContentType);
      if (extensions == null)
      {
        var fileSplit = request.FileName.Split('.');
        if (fileSplit.Length > 1)
        {
          return $".{fileSplit.LastOrDefault()}";
        }
      }
      return extensions.Extension;
    }
    public static string GetFileContentType(this FileServiceRequest request, bool isFileName = true)
    {
      var fileExtension = (isFileName ? request.FileName : request.FileUUID).Split('.').LastOrDefault();
      var extensions = Common.ContentExtensionMap.FirstOrDefault(b => b.Extension == $".{fileExtension}");
      if (extensions == null)
      {
        return extensions.ContentType;
      }
      return Common.ContentExtensionMap.FirstOrDefault().ContentType;
    }
    public static string GetFileUUID(this FileServiceRequest request)
    {
      return $"{Guid.NewGuid().ToString()}{request.GetFileExtension()}";
    }

    public static ContentResponse<bool> ValidateFile(this FileServiceOption options, string fileName, long fileSize)
    {
      var result = new ContentResponse<bool>();
      if (options.MaxFileUploadSize > 0 && fileSize > options.MaxFileUploadSize)
      {
        result.SetError(errorMessage: $"File size large than {options.MaxFileUploadSize}");
        return result;
      }
      if (!String.IsNullOrEmpty(options.AllowedFileType) &&
        !options.AllowedFileType.Split(",").Any(b => string.Equals(b, fileName.GetFileExtension(), StringComparison.OrdinalIgnoreCase)))
      {
        result.SetError(errorMessage: $"File type not allowed");
        return result;
      }
      result.SetSuccess(true);
      return result;
    }
  }
}
