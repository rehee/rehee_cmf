using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Contexts;
using ReheeCmf.Libs.Test.ContextsTest.Contexts;
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
      catch (StatusException ex)
      {
        v = true;
      }
      Assert.That(valiation, Is.EqualTo(v));
    }

  }
}
