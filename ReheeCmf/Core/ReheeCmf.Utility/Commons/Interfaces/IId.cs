namespace ReheeCmf.Commons
{
	public interface IId<T> where T : IEquatable<T>
	{
		T Id { get; set; }
	}
}
