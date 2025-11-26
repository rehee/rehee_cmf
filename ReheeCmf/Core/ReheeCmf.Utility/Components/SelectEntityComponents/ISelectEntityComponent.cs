using ReheeCmf.Handlers.SelectEntityHandlers;

namespace ReheeCmf.Components.SelectEntityComponents
{
	public interface ISelectEntityComponent : ICmfComponent
	{
		ISelectEntityHandler? GetSelectHandler();
	}
}
