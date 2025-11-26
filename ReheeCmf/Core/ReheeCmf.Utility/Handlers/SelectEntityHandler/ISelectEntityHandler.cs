using ReheeCmf.Attributes;
using ReheeCmf.Components;
using ReheeCmf.Contexts;
using ReheeCmf.Entities;

namespace ReheeCmf.Handlers.SelectHandler
{
	public interface ISelectEntityHandler : ICmfHandler
	{
		IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
	}

	public interface ISelectEntityComponent : ICmfComponent
	{
		ISelectEntityHandler? GetSelectHandler();
	}

	public class SelectEntityAttribute<TEntity> : CmfComponentAttribute, ISelectEntityComponent, IHandlerComponent
	{
		public override Type? EntityType => typeof(TEntity);
		public ISelectEntityHandler? GetSelectHandler()
		{
			return SingletonHandler() as ISelectEntityHandler;
		}

	}
	public abstract class SelectEntityHandler<T> : ISelectEntityHandler where T : class, ISelect
	{
		public abstract IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
	}
}
