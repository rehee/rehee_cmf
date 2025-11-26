using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace ReheeCmf.DTOProcessors.Processors
{
	public interface IDtoProcessor : ISaveChange, IValidatableObject, IFindByQueryKey, ITypeQuery, IDisposable, IAsyncDisposable, IDtoCreateProcessor, IDtoUpdateProcessor, IDtoDeleteProcessor
	{
		void Initialization(TokenDTO? user);
		void Initialization(string? json, TokenDTO? user);
		Task InitializationAsync(string? key, string? json, TokenDTO? user, CancellationToken ct);

	}
	public interface IDtoProcessor<T> : IDtoProcessor, ITypeQuery<T>, IFindByQueryKey<T>, ITypedCreateProcessor<T>, IDtoUpdateProcessor<T>, ITypedDeleteProcessor<T> where T : IQueryKey
	{

	}
}
