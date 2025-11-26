using System.Linq.Expressions;

namespace ReheeCmf.Attributes
{

	public class ReadCheckAttribute : Attribute
	{
	}
	public delegate Expression<Func<T, bool>> ReadCheck<T>(TokenDTO? user);
}
