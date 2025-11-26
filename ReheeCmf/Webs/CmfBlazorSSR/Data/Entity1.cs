using ReheeCmf.Attributes;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;
using ReheeCmf.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CmfBlazorSSR.Data
{
	public class Entity1 : EntityBase<long>
	{
	}
	[EntityChangeTracker<Entity1>]
	public class Entity1Handler : EntityChangeHandler<Entity1>
	{
		public override IEnumerable<ValidationResult> Validation()
		{
			return new List<ValidationResult>() { ValidationResultHelper.New("123") };
		}
	}
}
