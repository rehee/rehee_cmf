using ReheeCmf.Contexts;

namespace ReheeCmf.Handlers.SelectEntityHandlers
{
	public interface ISelectEntityHandler : ICmfHandler
	{
		IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
	}
}
