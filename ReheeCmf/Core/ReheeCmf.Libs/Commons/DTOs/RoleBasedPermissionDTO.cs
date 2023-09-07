using ReheeCmf.StandardInputs.Properties;
using System.Runtime.Serialization;

namespace ReheeCmf.Commons.DTOs
{
  [DataContract]
  public class RoleBasedPermissionDTO
  {
    [DataMember]
    public string? ModuleName { get; set; }
    [DataMember]
    public string? RoleName { get; set; }
    [DataMember]
    public StandardProperty[]? Items { get; set; }
  }
}
