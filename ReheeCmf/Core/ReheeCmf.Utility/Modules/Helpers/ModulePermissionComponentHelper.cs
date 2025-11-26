using ReheeCmf.DIContainers;
using ReheeCmf.Modules.Components;
using ReheeCmf.Modules.Permissions;

namespace ReheeCmf.Modules.Helpers
{
	public static class ModulePermissionComponentHelper
	{
		public static IEnumerable<string>? GetPermission<T>() where T : IModulePermission
		{
			return DIPool.ComponentPool.Values
				.Where(b => b is IModulePermissionComponent)
				.Where(b => b.EntityType == typeof(T))
				.SelectMany(b => (b as IModulePermissionComponent)!.GetSingletonPermissionHandler()!.ModulePermissions)
				.Distinct();

		}
	}
}
