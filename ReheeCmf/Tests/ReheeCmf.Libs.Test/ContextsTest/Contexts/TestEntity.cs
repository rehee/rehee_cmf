using ReheeCmf.Handlers.ChangeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.ContextsTest.Contexts
{
  internal class TestEntity : EntityBase<int>
  {
    public string? Name { get; set; }
    public string? Update { get; set; }
    public string? Before_Name { get; set; }
    public string? Before_Update { get; set; }
    public string? After_Name { get; set; }
    public string? After_Update { get; set; }

    public string? Create2 { get; set; }
    public string? Before_Create2 { get; set; }
  }
  internal class TestEntity2 : EntityBase<int>
  {
    public string? Name { get; set; }
  }
  internal class TestEntity3 : TestEntity
  {

  }



  [EntityChangeTracker<TestEntity, TestEntityHandler>]
  internal class TestEntityHandler : EntityChangeHandler<TestEntity>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity!.Before_Name = entity!.Name;

    }
    public override async Task AfterCreateAsync(CancellationToken ct = default)
    {
      await base.AfterCreateAsync(ct);
      entity!.After_Name = entity!.Name;

    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      entity!.Before_Update = entity!.Update;
    }
    public override async Task AfterUpdateAsync(CancellationToken ct = default)
    {
      await base.AfterUpdateAsync(ct);
      entity!.After_Update = entity!.Update;
    }
    public override Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      return base.BeforeDeleteAsync(ct);
    }
    public override async Task AfterDeleteAsync(CancellationToken ct = default)
    {
      await base.AfterDeleteAsync(ct);
      await context!.AddAsync<TestEntity2>(new TestEntity2
      {
        Name = entity!.Name,
      }, ct);
      await context!.SaveChangesAsync(null);
    }
  }

  [EntityChangeTracker<TestEntity, TestEntityHandler2>(NoInherit = true)]
  internal class TestEntityHandler2 : EntityChangeHandler<TestEntity>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity!.Before_Create2 = entity!.Create2;
    }
  }


  internal class TestDeleteItem : EntityBase<int>
  {
    public string? Name { get; set; }
  }
  [EntityChangeTracker<TestDeleteItem, TestDeleteItemHandler>]
  internal class TestDeleteItemHandler : EntityChangeHandler<TestDeleteItem>, IDeletedHandler
  {
    public bool IsDeleted { get; set; }

    public Task DeleteAsync(CancellationToken ct = default)
    {
      entity!.Name = "0";
      IsDeleted = false;
      return Task.CompletedTask;
    }
  }

}
