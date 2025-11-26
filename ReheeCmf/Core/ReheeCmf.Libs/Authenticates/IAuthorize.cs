namespace ReheeCmf.Authenticates
{
	public interface IAuthorize
	{
		Task<string> FullAccessRole();
		Task<bool> EnableAuth();
		Task<ContentResponse<TokenDTO>> ValidateAndConvert(string token);
		Task<string> ApiSystemToken();
	}
}
