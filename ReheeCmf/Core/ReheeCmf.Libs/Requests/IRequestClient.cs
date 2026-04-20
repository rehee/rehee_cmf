namespace ReheeCmf.Requests
{
	public interface IRequestClient : IRequestBase
	{
		Task<ContentResponse<T>> Request<T>(
			HttpMethod method, string url, string? json = null, Stream? content = null,
			Dictionary<string, string>? headValue = null, CancellationToken ct = default, bool ignoreToken = false);
	}
	public interface IRequestClient<T> : IRequestClient
	{
	}
}
