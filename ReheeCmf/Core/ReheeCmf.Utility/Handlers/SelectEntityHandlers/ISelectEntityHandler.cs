using ReheeCmf.Components;
using ReheeCmf.Contexts;
using ReheeCmf.Entities;

namespace ReheeCmf.Handlers.SelectEntityHandlers
{
  public interface ISelectEntityHandler : ICmfHandler
  {
    IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
  }
}
