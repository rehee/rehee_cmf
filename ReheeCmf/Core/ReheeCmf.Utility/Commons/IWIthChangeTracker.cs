namespace ReheeCmf
{
	public interface IWIthChangeTracker
	{
		string? ChangeTracker { get; set; }
		void Change();
	}
}
