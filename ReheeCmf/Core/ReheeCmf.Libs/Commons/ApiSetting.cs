using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public class ApiSetting
  {
    public bool IsDapr { get; set; }
    public string? EncryptionKey { get; set; }
    public RSAOption? RSAOption { get; set; }
    public IEnumerable<ServiceModuleOption>? ServiceOptions { get; set; }
  }
  
}
