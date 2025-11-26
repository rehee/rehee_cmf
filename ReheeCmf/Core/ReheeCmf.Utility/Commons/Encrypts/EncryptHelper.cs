using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Encrypts
{
  public static class EncryptHelper
  {
    public static byte[] GetKeyMd5Hash(this string key)
    {
      using (var hashmd5 = MD5.Create())
      {
        byte[] keyBytes = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        hashmd5.Clear();
        return keyBytes;
      }
    }
    public static string Encrypt(this string str, string keyInput)
    {


      using (var descsp = TripleDES.Create())
      {
        descsp.Key = (keyInput).GetKeyMd5Hash();
        descsp.Mode = CipherMode.ECB;
        descsp.Padding = PaddingMode.PKCS7;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(str);
        ICryptoTransform cTransform = descsp.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        descsp.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
      }
    }
    public static string Decrypt(this string str, string keyInput)
    {
      using (var descsp = TripleDES.Create())
      {
        descsp.Key = (keyInput).GetKeyMd5Hash();
        descsp.Mode = CipherMode.ECB;
        descsp.Padding = PaddingMode.PKCS7;
        byte[] enBytes = Convert.FromBase64String(str);
        ICryptoTransform cTransform = descsp.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(enBytes, 0, enBytes.Length);
        descsp.Clear();
        return Encoding.UTF8.GetString(resultArray);
      }
    }
  }
}
