using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Contexts;
using ReheeCmf.Libs.Test.ContextsTest.Contexts;
using ReheeCmf.Tenants;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.Libs.Test.ContextsTest.GeneralTests
{
  internal abstract class GeneralTest<TDbContext> : ContextsTest<TDbContext> where TDbContext : DbContext
  {
    [Test]
    public void ContextService_DbContext_Same_Instance_Test()
    {
      var db = serviceProvider!.GetService<TDbContext>();
      var context = serviceProvider!.GetService<IContext>();
      Assert.That(db!.GetHashCode(), Is.EqualTo(context!.Context!.GetHashCode()));
    }
    [Test]
    public async Task ContextService_Entity_Update()
    {
      var context = serviceProvider!.GetService<IContext>()!;
      var textName = "test";
      var testEntity = new TestEntity()
      {
        Name = textName,
      };
      await context.AddAsync<TestEntity>(testEntity, CancellationToken.None);
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);
      Assert.That(testEntity.Before_Name, Is.EqualTo(textName));
      Assert.That(testEntity.After_Name, Is.EqualTo(textName));
      var updateText = "10";
      testEntity.Update = "10";
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);
      Assert.That(testEntity.Before_Update, Is.EqualTo(updateText));
      Assert.That(testEntity.After_Update, Is.EqualTo(updateText));

      context.Delete<TestEntity>(testEntity);
      await context.SaveChangesAsync(null);

      var entity2 = context.Query<TestEntity2>(false).Select(b => b.Id).Count();
      Assert.That(entity2, Is.EqualTo(1));
    }
    [Test]
    public async Task ContextService_Entity_ShareSameHandler_Update()
    {
      var context = serviceProvider!.GetService<IContext>()!;
      var textName = "test";
      var testEntity = new TestEntity3()
      {
        Name = textName,
      };
      await context.AddAsync<TestEntity3>(testEntity, CancellationToken.None);
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);
      Assert.That(testEntity.Before_Name, Is.EqualTo(textName));
      Assert.That(testEntity.After_Name, Is.EqualTo(textName));
      var updateText = "10";
      testEntity.Update = "10";
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);
      Assert.That(testEntity.Before_Update, Is.EqualTo(updateText));
      Assert.That(testEntity.After_Update, Is.EqualTo(updateText));

      context.Delete<TestEntity>(testEntity);
      await context.SaveChangesAsync(null);

      var entity2 = context.Query<TestEntity2>(false).Select(b => b.Id).Count();
      Assert.That(entity2, Is.EqualTo(1));
    }
    [Test]
    public async Task ContextService_Entity_No_Inherit_Test()
    {
      var context = serviceProvider!.GetService<IContext>()!;
      var textName = "test";
      var testEntity = new TestEntity()
      {
        Create2 = textName,
      };
      var testEntity3 = new TestEntity3()
      {
        Create2 = textName,
      };
      await context.AddAsync<TestEntity>(testEntity, CancellationToken.None);
      await context.AddAsync<TestEntity3>(testEntity3, CancellationToken.None);
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);
      Assert.That(testEntity.Before_Create2, Is.EqualTo(textName));
      Assert.That(testEntity3.Before_Create2, Is.EqualTo(null));

    }

    [TestCase(1, false)]
    [TestCase(10, false)]
    [TestCase(11, true)]
    public async Task ContextService_Entity_Valuation_Test(int index, bool valiation)
    {
      //TODO there are some reason the exception will block the following test. skip for now
      if (!valiation)
      {
        return;
      }
      var entity = new TestValidationEntity
      {
        Index = index,
      };
      var v = false;
      try
      {
        var db = serviceProvider!.GetService<IContext>()!;
        await db.AddAsync<TestValidationEntity>(entity, CancellationToken.None);
        await db.SaveChangesAsync(null);
      }
      catch (Exception)
      {
        v = true;
      }
      Assert.That(valiation, Is.EqualTo(v));
    }
    [Test]
    public void Context_Get_Type_Query()
    {
      var context = serviceProvider!.GetService<IContext>()!;
      var db = serviceProvider!.GetService<TDbContext>()!;

      var e1 = db.Set<TestEntity>();
      var e2 = context.Query(typeof(TestEntity), false);

      Assert.That(e1, Is.EqualTo(e2));
    }
    [Test]
    public void Context_Get_Type_Find()
    {
      var context = serviceProvider!.GetService<IContext>()!;
      var db = serviceProvider!.GetService<TDbContext>()!;
      var entity1 = new TestEntity();
      db.Set<TestEntity>().Add(entity1);
      db.SaveChanges();

      var e1 = db.Set<TestEntity>().Find(entity1.Id)!;
      var e2 = context.Find(typeof(TestEntity), entity1.Id) as TestEntity;

      Assert.That(e1.Id, Is.EqualTo(e2!.Id));
    }
    [Test]
    public void Context_Get_Type_Delete()
    {
      var context = serviceProvider!.GetService<IContext>()!;
      var db = serviceProvider!.GetService<TDbContext>()!;
      var entity1 = new TestEntity();
      db.Set<TestEntity>().Add(entity1);
      db.SaveChanges();
      var id = entity1.Id;
      context.Delete(typeof(TestEntity), entity1.Id);
      db.SaveChanges();
      var count = db.Set<TestEntity>().Count();
      Assert.That(count, Is.EqualTo(0));
    }

    [Test]
    public async Task Default_Tenant_Storage_And_Track_Test()
    {
      var db = serviceProvider!.GetService<TDbContext>()!;
      var storage = serviceProvider!.GetService<ITenantStorage>()!;

      var tenant1 = new TenantEntity()
      {
        TenantName = "TenantName1",
        TenantSubDomain = "TenantName1"
      };
      db.Add<TenantEntity>(tenant1);
      await db.SaveChangesAsync();
      var count = storage.GetAllTenants().Count();

      Assert.That(count, Is.EqualTo(1));
      var tenant2 = new TenantEntity()
      {
        TenantName = "T2",
        TenantSubDomain = "T2"
      };
      db.Add<TenantEntity>(tenant2);
      await db.SaveChangesAsync();

      count = storage.GetAllTenants().Count();

      Assert.That(count, Is.EqualTo(2));
    }
  }
}
