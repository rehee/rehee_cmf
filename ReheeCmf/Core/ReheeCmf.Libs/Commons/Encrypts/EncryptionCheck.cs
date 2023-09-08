using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Encrypts
{
  public class EncryptionCheck
  {
    public static EncryptionCheck New()
    {
      return new EncryptionCheck(Guid.NewGuid(), 0);
    }

    public static string GetToken(string publicKey)
    {
      return New().GetRequestToken(publicKey);
    }
    public static bool ValidateToken(string token)
    {
      if (string.IsNullOrEmpty(token))
      {
        return false;
      }
      var tokens = token.Trim().Split(",").Select(b => b.Trim()).ToArray();
      if (tokens.Length != 2)
      {
        return false;
      }
      if (!Guid.TryParse(tokens[0], out var guidID))
      {
        return false;
      }
      if (!long.TryParse(tokens[1], out var answer))
      {
        return false;
      }
      return new EncryptionCheck(guidID, answer).Validation();
    }

    public EncryptionCheck()
    {

    }
    public EncryptionCheck(Guid id, long answer)
    {
      var strs = id.ToString().Split("-")
        .Select(b => $"0x{b}")
        .Select(b => Convert.ToUInt64(b, 16)).ToArray();

      thisGuid = id;
      First = (int)(strs[0] % 1000);
      Last = (int)(strs[1] % 1000);
      Option = (int)(strs[2] % 4);
      Answer = answer;
    }
    private Guid thisGuid { get; set; }
    public int First { get; set; }
    public int Last { get; set; }
    public int Option { get; set; }
    public long Answer { get; set; }

    public string GetRequestToken(string publicKey)
    {
      return $"{thisGuid.ToString()},{GetAnswer()}".RSAEncrypt(publicKey);
    }
    public bool Validation()
    {
      return Answer == GetAnswer();
    }
    public long GetAnswer()
    {
      switch (Option)
      {
        case 0:
          return First + Last;

        case 1:
          return First - Last;

        case 2:
          return First * Last;

        default:
          return Last == 0 ? 0 : First % Last;
      }
    }
  }
}
