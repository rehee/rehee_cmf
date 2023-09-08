using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Contexts
{
  public interface IWithContext
  {
    IContext? Context { get; set; }
  }
}
