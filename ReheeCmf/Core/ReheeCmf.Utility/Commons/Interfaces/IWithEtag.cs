using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public interface IWithEtag
  {
    byte[] ETag { get; set; }
  }
}
