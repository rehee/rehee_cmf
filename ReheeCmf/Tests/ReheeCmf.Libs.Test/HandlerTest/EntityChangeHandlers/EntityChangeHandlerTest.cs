namespace ReheeCmf.Libs.Test.HandlerTest.EntityChangeHandlers
{
	public class EntityChangeHandlerTest
	{
		[Test]
		public async Task HandlerTest()
		{
			var obj = new TestEntity();
			var handler = obj.GetType().GetComponentsByHandler<TestEntityHandler>()!;
			var hd = handler.FirstOrDefault()!.CreateHandler<TestEntityHandler>();
			var validationResult = hd?.Validation();
			Assert.That(validationResult?.Count(), Is.EqualTo(1));
		}


	}
	[EntityChange<TestEntityHandler>]
	file class TestEntity : EntityBase<int>
	{

	}
	file class TestEntityHandler : EntityChangeHandler<TestEntity>
	{
		public override IEnumerable<ValidationResult> Validation()
		{
			var result = new ValidationResult[]
			{
				new ValidationResult("",new string[]{ })
			};
			return result;
		}
	}
}
