using ReheeCmf.Commons.Consts;
using Microsoft.Extensions.Primitives;
namespace ReheeCmf.Helpers
{
  public static class IHttpDictionaryHelper
  {
    public static ContentResponse<string> GetAccessToken(this IDictionary<string, StringValues> input)
    {
      var result = new ContentResponse<string>();
      if (input.TryGetValue(ConstOptions.AuthorizeHeader, out var t))
      {
        result.SetSuccess(t);
      }
      return result;
    }
  }
}
