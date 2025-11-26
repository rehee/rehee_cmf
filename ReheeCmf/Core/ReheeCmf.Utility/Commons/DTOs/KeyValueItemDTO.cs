using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  [DataContract]
  public class KeyValueItemDTO
  {
    [DataMember]
    public string? Key { get; set; }
    [DataMember]
    public string? Value { get; set; }
    [DataMember]
    public bool Selected { get; set; }
  }
}
