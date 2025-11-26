namespace ReheeCmf.Handlers.ChangeHandlers
{

	public interface IDeletedHandler
	{
		bool IsDeleted { get; }
		void Delete();
	}
}
