using ReheeCmf.DIContainers;
using ReheeCmf.Helpers;

namespace ReheeCmf.ODatas.Components
{
	public static class ODataEntitySetFactory
	{
		public static IODataEntitySetHandler? GetHandler(Type entityType)
		{
			var allComponent = DIPool.ComponentPool.Values.ToArray();
			return DIPool.ComponentPool.Where(b =>
			{
				if (b.Value is IODataEntitySet s)
				{
					return entityType.IsImplement(s.EntityType);

				}
				return false;
			}).OrderByDescending(b => b.Value.Index).ThenByDescending(b => b.Value.SubIndex)
			.Select(b =>
			{
				if (b.Value is IODataEntitySet s)
				{
					return s.GetHandler(entityType);
				}
				return null;
			}).FirstOrDefault();
		}
		public static IEnumerable<IODataEntitySetHandler>? GetHandlers(Type entityType)
		{
			var allComponent = DIPool.ComponentPool.Values.ToArray();
			return DIPool.ComponentPool.Where(b =>
			{
				if (b.Value is IODataEntitySet s)
				{
					return s.EntityType != null && entityType.IsInheritance(s.EntityType);

				}
				return false;
			}).OrderByDescending(b => b.Value.Index).ThenByDescending(b => b.Value.SubIndex)
			.Select(b =>
			{
				if (b.Value is IODataEntitySet s)
				{
					return s.GetHandler(entityType);
				}
				return null;
			})
			.Where(b => b != null)
			.Select(b => b!);

		}
	}
}
