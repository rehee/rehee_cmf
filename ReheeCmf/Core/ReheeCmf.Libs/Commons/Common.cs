namespace System
{
  public partial class Common
  {
    public const string DataFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
    public const string DataFormat2 = "yyyy-MM-ddTHH:mm:ss";
    public const string DigitalFormat = "#0.0000";
    public const string StringDelimiter = "[__,__]";
    public const string SystemApiClaimType = "SystemInternalApiCall";
    public const string SystemApiClaimValue = "True";
    public const string FileEndpoint = "File/uuid";
    public const int IdIndex = -1;
    public const string ErrorCode_Validation = "Validation";
    public static string[] Proxies = { "Castle.Proxies" };
    public static Type IgnoreProperty = typeof(IgnoreUpdateAttribute);
    public static string TemplatepPttern = "\\[[^\\[]+\\]";
    public static string TenantIDHeader = "TenantID";
    public static string TenantNameHeader = "TenantName";
    public static string TenantUrlName = "tenant";
    public static string ClientUrlHeader = "ClientUrl";
    public static string EmptyGuid = "00000000-0000-0000-0000-000000000000";

    public static string[] EmptyString = new string[]
    {
      " ","\r","\n","\r\n","\t"
    };
  }
}
