using ReheeCmf.Attributes;
using System.Reflection;

namespace ReheeCmf.DIContainers
{
	public static partial class DIPool
	{
		private static Action<Attribute, Type>[] Registers { get; set; } =
		[
			RegisterComponent
		];
		private static void RegisterAll(this IEnumerable<Attribute> attributes, Type decorate)
		{
			foreach (var attribute in attributes.Where(b => b is IRegistrableAttribute))
			{
				foreach (var r in Registers)
				{
					r(attribute, decorate);
				}
			}
		}
		public static void InitRegistrableAttribute(Type type, params CustomAttributeData[] attributes)
		{
			if (attributes.Any(b => b.AttributeType.IsImplement<IRegistrableAttribute>()))
			{
				Attribute.GetCustomAttributes(type).RegisterAll(type);
			}
			RegistController(type);
			foreach (var property in type.GetProperties())
			{

			}
			foreach (var field in type.GetFields())
			{

			}
		}
	}
}
