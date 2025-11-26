namespace ReheeCmf.Authenticates
{
	public interface IDTOSignInManager<TDTO>
	{
		Task<ContentResponse<bool>> LoginAsync(TDTO dto, CancellationToken ct, bool checkPassword = true);
		Task<ContentResponse<bool>> LogoutAsync(CancellationToken ct);
	}
}
