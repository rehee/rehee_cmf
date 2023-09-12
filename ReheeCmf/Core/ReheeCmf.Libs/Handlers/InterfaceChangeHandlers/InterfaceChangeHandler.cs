using ReheeCmf.Handlers.ChangeHandlerss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.InterfaceChangeHandlers
{
  public interface IInterfaceChangeHandler : IChangeHandler
  {

  }
  public class InterfaceChangeHandler<T> : ChangeHandler<T>, IInterfaceChangeHandler
  {

    public override Task SetTenant(CancellationToken ct = default)
    {
      return Task.CompletedTask;
    }
  }
}
