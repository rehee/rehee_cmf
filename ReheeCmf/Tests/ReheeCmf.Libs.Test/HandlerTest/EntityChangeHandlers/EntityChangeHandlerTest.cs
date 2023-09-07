using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.HandlerTest.EntityChangeHandlers
{
  public class EntityChangeHandlerTest
  {
    [Test]
    public async Task HandlerTest()
    {
      var obj = new TestEntity();
      var handler = obj.GetType().GetTypedAttribute<EntityChangeHandlerAttribute>()!;
      var hd = handler.GetHandler()!;
      var validationResult = await hd.ValidationAsync();
      Assert.That(validationResult.Count(), Is.EqualTo(1));
    }


  }
  [EntityChangeHandler<TestEntityHandler>]
  file class TestEntity : EntityBase<int>
  {

  }
  file class TestEntityHandler : EntityChangeHandler<TestEntity>
  {
    public override Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      var result = new ValidationResult[]
      {
        new ValidationResult("",new string[]{ })
      };
      return Task.FromResult(result as IEnumerable<ValidationResult>);
    }
  }
}
