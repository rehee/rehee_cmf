using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  [DataContract]
  public class LoginDTO
  {
    [DataMember]
    [Required]
    public string? Username { get; set; }
    [DataMember]
    [Required]
    public string? Password { get; set; }
    [DataMember]
    public bool KeepLogin { get; set; }
    [DataMember]
    public string? ReturnUrl { get; set; }
  }
}
