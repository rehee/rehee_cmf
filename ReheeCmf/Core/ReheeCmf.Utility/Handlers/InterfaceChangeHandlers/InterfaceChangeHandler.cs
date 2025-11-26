using ReheeCmf.Handlers.ChangeHandlers;

namespace ReheeCmf.Handlers.InterfaceChangeHandlers
{
	public interface IInterfaceChangeHandler : IChangeHandler
	{

	}
	public class InterfaceChangeHandler<T> : ChangeHandler<T>, IInterfaceChangeHandler
	{

		public override void SetTenant()
		{

		}
	}
}
