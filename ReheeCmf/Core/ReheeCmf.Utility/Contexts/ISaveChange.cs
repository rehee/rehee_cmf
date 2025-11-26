using ReheeCmf.Commons.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace ReheeCmf.Contexts
{
	public interface ISaveChange
	{
		int SaveChanges(TokenDTO? user = null);
		Task<int> SaveChangesAsync(TokenDTO? user = null, CancellationToken ct = default);
	}
}
