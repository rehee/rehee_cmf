

namespace ReheeCmf.Libs.Test.ContextsTest.GeneralTests
{
  internal class EntityValidationTest1 : EntityValidationTest<TestDbContext>
  {
  }
  internal class EntityValidationTest2 : EntityValidationTest<TestDbContext2>
  {
  }
  internal abstract class EntityValidationTest<TDbContext> : ContextsTest<TDbContext> where TDbContext : DbContext
  {
    protected virtual IServiceProvider sp { get; set; }
    [SetUp]
    public override void Setup()
    {
      base.Setup();
      sp = ConfigService();
    }

    [Test]
    public void TestWithWhiteClass()
    {
      var obj = new WhiteClass();
      var validationContext = new ValidationContext(obj);
      var result = new List<ValidationResult>();
      Validator.TryValidateObject(obj, validationContext, result);
      Assert.That(result.Count, Is.EqualTo(1));
    }
    [Test]
    public void TestWithWhiteEntityClass()
    {
      using var db = sp.GetService<IContext>();
      var entity = new WhiteClass();
      var throwEx = false;
      StatusException? exTherow = null;

      try
      {
        db.Add(typeof(WhiteClass), entity);
        db.SaveChanges(null);
      }
      catch (StatusException ex)
      {
        throwEx = true;
        exTherow = ex;
      }
      catch (Exception ex2)
      {
        var exConvert = ex2.GetStatusException();
        if (exConvert != null)
        {
          throwEx = true;
          exTherow = exConvert;
        }
      }
      Assert.That(throwEx, Is.True);
      Assert.NotNull(exTherow);
      Assert.That(exTherow.ValidationError!.Count(), Is.EqualTo(1));
    }
  }


  internal class WhiteClass : EntityBase<int>
  {
    [Required]
    public string? Name { get; set; }
  }
  [EntityChangeTracker<WhiteClass>]
  internal class WhiteClassHandler : EntityChangeHandler<WhiteClass>
  {
    public override Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      return base.ValidationAsync(ct);
    }
  }
}
