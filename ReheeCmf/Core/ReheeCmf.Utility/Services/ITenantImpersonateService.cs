using ReheeCmf.Entities;

namespace ReheeCmf.Services
{

	public interface ITenantImpersonateService<TUser> where TUser : ICmfUser
	{
		Task<bool> CheckImpersonateAsync(TUser user);
		Task<TUser?> GetImpersonateUserAsync(string userName, string? password = null);
	}

	public interface IImpersonateManagement
	{
		Task<TokenDTO> GetImpersonateTokenAsync(LoginDTO loginDTO, CancellationToken ct);
	}
}
