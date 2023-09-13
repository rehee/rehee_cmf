using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.ChangeHandlers
{

  public interface IDeletedHandler
  {
    bool IsDeleted { get; }
    Task DeleteAsync(CancellationToken ct = default);
  }
}
