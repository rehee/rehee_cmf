using ReheeCmf.Contexts;
using ReheeCmf.Entities;

namespace ReheeCmf.Handlers.SelectEntityHandlers
{
  public abstract class SelectEntityHandler<T> : ISelectEntityHandler where T : class, ISelect
  {
    public abstract IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
  }
}
