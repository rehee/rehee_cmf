namespace ReheeCmf.Libs.Test.ContextsTest.Contexts
{
	internal class TestValidationEntity : EntityBase<int>
	{
		public int Index { get; set; }
	}

	[EntityChangeTracker<TestValidationEntity>]
	internal class TestValidationEntityHandler : EntityChangeHandler<TestValidationEntity>
	{
		public override IEnumerable<ValidationResult> Validation()
		{
			return (base.Validation()).Concat(_validationResults());

		}
		private IEnumerable<ValidationResult> _validationResults()
		{
			if (entity!.Index > 10)
			{
				yield return ValidationResultHelper.New("Index is more than 10", nameof(Index));
			}
		}
	}

}
