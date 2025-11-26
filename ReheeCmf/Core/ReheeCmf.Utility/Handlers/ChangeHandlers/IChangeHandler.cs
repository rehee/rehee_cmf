using ReheeCmf.Components;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.ValidationHandlers;

namespace ReheeCmf.Handlers.ChangeHandlers
{
	public interface IChangeHandler : IValidationHandler, IDisposable, ICmfHandler
	{
		int EntityHashCode { get; }
		int Index { get; }
		int SubIndex { get; }
		string? Group { get; }

		EnumEntityState EntityState { get; }

		void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null);
		void BeforeCreate();
		void BeforeUpdate(EntityChanges[] propertyChange);
		void BeforeDelete();

		Task AfterCreateAsync(CancellationToken ct = default);
		Task AfterUpdateAsync(CancellationToken ct = default);
		Task AfterDeleteAsync(CancellationToken ct = default);
	}
}
