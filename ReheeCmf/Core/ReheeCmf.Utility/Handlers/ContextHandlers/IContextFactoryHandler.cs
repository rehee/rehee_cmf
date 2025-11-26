using ReheeCmf.Components;
using ReheeCmf.Contexts;

namespace ReheeCmf.Handlers.ContextHandlers
{
	public interface IContextFactoryComponent : ICmfComponent
	{

	}


	public class ContextFactoryComponentAttribute<T> : CmfComponentAttribute, IEntityComponent, IContextFactoryComponent where T : ICmfHandler, new()
	{
		public override Type? HandlerType => typeof(T);
	}
	public interface IContextFactoryHandler : ICmfHandler
	{
		IContext CreateContext(IServiceProvider serviceProvider);
	}
}
