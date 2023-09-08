namespace ReheeCmf.Helper
{
  public static class ETagHelper
  {
    public static string EncodeETag(this IWithEtag input)
    {
      if (input?.ETag?.Any() != true)
      {
        return null;
      }
      return Convert.ToBase64String(input.ETag);
    }
    public static string EncodeETagString(this byte[] input)
    {
      if (input == null)
      {
        return null;
      }
      return Convert.ToBase64String(input);
    }
    public static byte[] StringToEtag(this string input)
    {
      if (input == null)
      {
        return new byte[0];
      }
      return Convert.FromBase64String(input.Replace("\"", ""));
    }
  }
}
