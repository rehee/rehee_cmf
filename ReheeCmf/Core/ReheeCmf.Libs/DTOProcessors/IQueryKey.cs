using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.DTOProcessors
{
  public interface IQueryKey
  {
    string? QueryKey { get; set; }
  }

  public interface IQueryId<T>
  {
    T? Id { get; set; }
    EnumIdType IdType { get; }
  }

}
