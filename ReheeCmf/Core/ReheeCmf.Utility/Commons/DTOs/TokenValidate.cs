using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  [DataContract]
  public class TokenValidate
  {
    [DataMember]
    public string? Token { get; set; }
  }
}
