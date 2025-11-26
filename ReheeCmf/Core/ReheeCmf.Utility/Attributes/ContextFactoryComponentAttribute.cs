using ReheeCmf.Components.ContextFactoryComponents;
using ReheeCmf.Handlers;

namespace ReheeCmf.Attributes
{
	public class ContextFactoryComponentAttribute<T> : CmfComponentAttribute, IEntityComponent, IContextFactoryComponent where T : ICmfHandler, new()
	{
		public override Type? HandlerType => typeof(T);
	}
}
