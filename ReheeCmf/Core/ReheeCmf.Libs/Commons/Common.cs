using System.Globalization;

namespace System
{
  public partial class Common
  {
    public const string DataFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
    public const string DataFormat2 = "yyyy-MM-ddTHH:mm:ss";
    public const string DATE = "yyyy-MM-dd";
    public const string DATETIME = "yyyy-MM-ddTHH:mm:ss.fff";
    public const string DATETIME2 = "yyyy-MM-ddTHH:mm:ss";
    public const string DATETIMEUTC = "yyyy-MM-ddTHH:mm:ss.fffZ";
    public const string DATETIMELOCAL = "yyyy-MM-ddTHH:mm:ss.fffzzz";
    public const string DATEGMT = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";
    public static readonly string[] DateTimeFormats = { DATETIMEUTC, DATETIMELOCAL, DATETIME, DATETIME2, DATE, DATEGMT };

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

    public static CultureInfo Culture => CultureInfo.GetCultureInfo(CultureName ?? DefaultCulture);
    public static string? CultureName { get; set; }
    public const string DefaultCulture = "en-GB";
    public static string[] EmptyString = new string[]
    {
      " ","\r","\n","\r\n","\t"
    };
  }
}
