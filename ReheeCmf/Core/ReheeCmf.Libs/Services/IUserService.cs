using ReheeCmf.StandardInputs.StandardItems;
using System.Collections;

namespace ReheeCmf.Services
{
  public interface IUserService
  {
    IEnumerable ReadUsers(string? role, TokenDTO? user = null);
    IEnumerable? GetUserRoles(string? userId, TokenDTO? user = null);

    Task<StandardItem?> GetUserDetailAsync(string? userId, TokenDTO? user = null, CancellationToken ct = default);
    Task<string> CreateUserAsync(Dictionary<string, string?> properties, TokenDTO? user = null);
    Task<bool> UpdateUserAsync(string id, Dictionary<string, string?> properties, TokenDTO? user = null, CancellationToken ct = default);

    IEnumerable GetAllRoles();
    IEnumerable? GetAllUserRoles();

    Task<string> CreateRoleAsync(Dictionary<string, string?> data, TokenDTO? user, CancellationToken ct = default);
    Task<bool> DeleteRoleAsync(string idOrName, TokenDTO? user, CancellationToken ct = default);
    
    Task<bool> ResetPassword(string userId, string? password, TokenDTO? user, bool forgot = false, string? token = null);

    

    Task<bool> ChangePasswordAsync(ChangePasswordDTO? dto, TokenDTO? user, CancellationToken ct = default);
  }
}
