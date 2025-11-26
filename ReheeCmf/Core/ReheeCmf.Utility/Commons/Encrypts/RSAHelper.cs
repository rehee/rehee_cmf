using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Encrypts
{
  public static class RSAHelper
  {
    public const int LengthPerKey = 50;
    private const string split_mark = "]_._[";
    public static string RSAEncrypt(this string text, string publickey)
    {

      using (var rsa = new RSACryptoServiceProvider())
      {
        var groups = text.Length / LengthPerKey + (text.Length % LengthPerKey != 0 ? 1 : 0);
        var subString = text.Select((x, i) => (x, i % groups))
          .GroupBy(b => b.Item2)
          .OrderBy(b => b.Key)
          .Select(b => String.Join("", b.OrderBy(b => b.Item2).Select(c => c.x)))
          .ToArray();
        rsa.FromXmlString(publickey);

        var subEncrype = subString.Select(b =>
        {
          byte[] buffer = rsa.Encrypt(
         Encoding.Unicode.GetBytes(b), false);
          return Convert.ToBase64String(buffer);
        }).ToArray();
        return String.Join(split_mark, subEncrype);
      }
    }
    public static string RSADecrypt(this string text, string privateKey)
    {
      using (var rsa = new RSACryptoServiceProvider())
      {

        var subText = text.Split(split_mark).Select(b =>
        {
          var bytesa = Convert.FromBase64String(b);
          rsa.FromXmlString(privateKey);
          byte[] buffer = rsa.Decrypt(bytesa
            , false);
          return Encoding.Unicode.GetString(buffer);
        }).ToArray();
        var b = new List<char>();
        for (var i = 0; i < subText[0].Length; i++)
        {
          foreach (var r in subText)
          {
            if (i >= r.Length)
            {
              continue;
            }
            b.Add(r[i]);
          }
        }
        var c = String.Join("", b);
        return c;
      }

    }
  }
}
