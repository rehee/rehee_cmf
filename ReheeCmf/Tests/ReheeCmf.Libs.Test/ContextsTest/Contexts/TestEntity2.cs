using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.ContextsTest.Contexts
{
  internal class TestValidationEntity : EntityBase<int>
  {
    public int Index { get; set; }
  }

  [EntityChangeTracker<TestValidationEntity>]
  internal class TestValidationEntityHandler : EntityChangeHandler<TestValidationEntity>
  {
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      return (await base.ValidationAsync(ct)).Concat(_validationResults());

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
