namespace ReheeCmf.FileServices
{
  public class FileServiceOption
  {
    public const string FileApi = "File";
    public const string FileServiceApi = "Api/FileService";
    public const string DefaultBaseFolder = "Files";
    public const string DefaultServerPath = "";
    public EnumFileService ServiceType { get; set; }
    public bool Compression { get; set; }
    public string? CompressionFileExtensions { get; set; }
    public string? BaseFolder { get; set; }
    public string? ServerPath { get; set; }
    public string? AccessToken { get; set; }
    public long? MaxFileUploadSize { get; set; }
    public string? AllowedRoles { get; set; }
    public bool? AuthRequired { get; set; }
    public string? AllowedFileType { get; set; }
    public static FileServiceOption New(TenantEntity tenant, FileServiceOption option)
    {
      return new FileServiceOption
      {
        ServiceType = tenant.FileServiceType ?? option.ServiceType,
        Compression = tenant.FileCompression ?? option.Compression,
        CompressionFileExtensions = tenant.FileCompressionFileExtensions ?? option.CompressionFileExtensions,
        BaseFolder = tenant.FileBaseFolder ?? option.BaseFolder,
        ServerPath = tenant.FileServerPath ?? option.ServerPath,
        AccessToken = tenant.FileAccessToken ?? option.AccessToken,
        MaxFileUploadSize = tenant.FileMaxFileUploadSize ?? option.MaxFileUploadSize,
        AllowedRoles = tenant.FileAllowedRoles ?? option.AllowedRoles,
        AuthRequired = tenant.FileAuthRequired ?? option.AuthRequired,
        AllowedFileType = tenant.FileAllowedFileType ?? option.AllowedFileType,
      };
    }
  }
}
