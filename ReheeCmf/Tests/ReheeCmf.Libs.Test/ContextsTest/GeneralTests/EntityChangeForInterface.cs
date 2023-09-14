using ReheeCmf.Handlers.InterfaceChangeHandlers;
using System.Runtime.InteropServices;

namespace ReheeCmf.Libs.Test.ContextsTest.GeneralTests
{
  internal class EntityChangeForInterface1 : EntityChangeForInterface<TestDbContext>
  {
  }
  internal class EntityChangeForInterface2 : EntityChangeForInterface<TestDbContext2>
  {
  }
  internal abstract class EntityChangeForInterface<TDbContext> : ContextsTest<TDbContext> where TDbContext : DbContext
  {
  }


  public class InterfaceHanderEntity : EntityBase<int>, ITest
  {
    public string? Name { get; set; }
    public string? Name_Before { get; set; }
    public string? Name_After { get; set; }
  }
  internal interface ITest
  {
    string? Name { get; set; }
    string? Name_Before { get; set; }
    string? Name_After { get; set; }
  }
  [InterfaceChangeTracker<ITest>]
  internal class TestInterfaceHandler : InterfaceChangeHandler<ITest>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.Name_Before = $"{entity.Name ?? ""}before";

    }
    public override async Task AfterCreateAsync(CancellationToken ct = default)
    {
      await base.AfterCreateAsync(ct);
      entity.Name_After = $"{entity.Name ?? ""}after";
    }
  }
}
