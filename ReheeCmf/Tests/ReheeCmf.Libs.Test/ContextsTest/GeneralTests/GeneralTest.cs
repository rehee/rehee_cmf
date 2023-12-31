﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Caches.MemoryCaches;
using ReheeCmf.Contexts;
using ReheeCmf.Entities;
using ReheeCmf.Libs.Test.ContextsTest.Contexts;
using ReheeCmf.Tenants;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.Libs.Test.ContextsTest.GeneralTests
{
  internal class TestDbContextTest : GeneralTest<TestDbContext>
  {
  }
  internal class TestDbContext2Test : GeneralTest<TestDbContext2>
  {
  }
  internal abstract class GeneralTest<TDbContext> : ContextsTest<TDbContext> where TDbContext : DbContext
  {
    private IServiceProvider sp { get; set; }
    public override IServiceProvider ConfigService(params Action<IServiceCollection>[] actions)
    {
      sp = base.ConfigService(actions);
      return sp;
    }

    [Test]
    public void ContextService_DbContext_Same_Instance_Test()
    {
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
      Assert.That(db!.GetHashCode(), Is.EqualTo(context!.Context!.GetHashCode()));
    }
    [Test]
    public async Task ContextService_Entity_Update()
    {
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
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
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
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
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
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
    [TestCase(12, true)]
    public async Task ContextService_Entity_Valuation_Test(int index, bool valiation)
    {
      var serviceProvider = ConfigService();
      var entity = new TestValidationEntity
      {
        Index = index,
      };
      var v = false;
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
      try
      {

        await context.AddAsync<TestValidationEntity>(entity, CancellationToken.None);
        await context.SaveChangesAsync(null);
      }
      catch (Exception)
      {
        v = true;
      }
      finally
      {
        db.Dispose();
      }
      Assert.That(valiation, Is.EqualTo(v));
    }
    [Test]
    public void Context_Get_Type_Query()
    {
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;

      var e1 = db.Set<TestEntity>();
      var e2 = context.Query(typeof(TestEntity), false);

      Assert.That(e1, Is.EqualTo(e2));
    }
    [Test]
    public void Context_Get_Type_Find()
    {
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
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
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
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
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
      var storage = serviceProvider.GetService<ITenantStorage>();
      storage?.ClearCashed();
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
    [Test]
    public async Task Entity_Name_Test()
    {
      var serviceProvider = ConfigService();
      var name = nameof(TestEntity).ToLower();
      var entityInfo = EntityRelationHelper.GetEntityTypeAndKey(name)!;
      Assert.That(entityInfo.Value.entityType, Is.EqualTo(typeof(TestEntity)));
      Assert.That(entityInfo.Value.keyType, Is.EqualTo(typeof(int)));
      Assert.That(entityInfo.Value.entityName, Is.EqualTo(nameof(TestEntity)));

      var newEntity = Activator.CreateInstance(entityInfo.Value.entityType);
      using var db = serviceProvider!.GetService<IContext>()!;
      using var context = serviceProvider!.GetService<TDbContext>()!;
      db.Add(entityInfo.Value.entityType, newEntity);
      db.SaveChanges(null);
      var count = db.Query<TestEntity>(false).Select(b => b.Id).ToList();
      Assert.That(count.Count, Is.EqualTo(1));
    }
    [Test]
    public void Interface_Name_Test()
    {
      var name = Guid.NewGuid().ToString();
      var entityName = nameof(InterfaceHanderEntity);
      var serviceProvider = ConfigService();
      var entityInfo = EntityRelationHelper.GetEntityTypeAndKey(entityName)!;
      Assert.That(entityInfo.Value.entityType, Is.EqualTo(typeof(InterfaceHanderEntity)));
      Assert.That(entityInfo.Value.keyType, Is.EqualTo(typeof(int)));
      Assert.That(entityInfo.Value.entityName, Is.EqualTo(nameof(InterfaceHanderEntity)));
      var newEntity = Activator.CreateInstance(entityInfo.Value.entityType) as InterfaceHanderEntity;
      newEntity!.Name = name;
      using var db = serviceProvider!.GetService<IContext>()!;
      using var context = serviceProvider!.GetService<TDbContext>()!;
      db.Add(entityInfo.Value.entityType, newEntity);
      db.SaveChanges(null);
      Assert.True(newEntity!.Name_After!.EndsWith("After", StringComparison.OrdinalIgnoreCase));
      var count = db.Query<InterfaceHanderEntity>(false).Where(b => b.Id > 0).ToList();
      Assert.That(count.Count, Is.EqualTo(1));
      var check = count.FirstOrDefault() as InterfaceHanderEntity;

      //Assert.True(check!.Name_After!.EndsWith("After", StringComparison.OrdinalIgnoreCase));
      Assert.True(check!.Name_Before!.EndsWith("Before", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task Entity_Override_Delete()
    {
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
      var textName = "test";
      var testEntity = new TestDeleteItem()
      {

      };
      await context.AddAsync<TestDeleteItem>(testEntity, CancellationToken.None);
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);
      var entity2 = context.Query<TestDeleteItem>(false).Select(b => b.Id).Count();
      Assert.That(entity2, Is.EqualTo(1));


      context.Delete<TestDeleteItem>(testEntity);
      var entityName = nameof(InterfaceHanderEntity);
      var entityInfo = EntityRelationHelper.GetEntityTypeAndKey(entityName)!;
      var newEntity = Activator.CreateInstance(entityInfo.Value.entityType) as InterfaceHanderEntity;

      context.Add(entityInfo.Value.entityType, newEntity);
      await context.SaveChangesAsync(null);
      entity2 = context.Query<TestDeleteItem>(false).Select(b => b.Id).Count();
      Assert.That(entity2, Is.EqualTo(1));

      var entity3 = context.Query<InterfaceHanderEntity>(false).Select(b => b.Id).Count();
      Assert.That(entity3, Is.EqualTo(1));

      var newDeletedEntity = context.Query<TestDeleteItem>(false).Where(b => b.Name == "0").Count();
      Assert.That(newDeletedEntity, Is.EqualTo(1));
    }
    [Test]
    public async Task Entity_Select_Test()
    {
      var serviceProvider = ConfigService();
      using var context = serviceProvider!.GetService<IContext>()!;
      using var db = serviceProvider!.GetService<TDbContext>()!;
      for (var i = 1; i < 21; i++)
      {
        var textName = "test" + i.ToString();
        var testEntity = new TestEntity3()
        {
          Name = textName,
        };
        await context.AddAsync<TestEntity3>(testEntity, CancellationToken.None);
      }
      await context.SaveChangesAsync(null);
      await Task.Delay(1000);

      var select = context.GetKeyValueItemDTO(typeof(TestEntity3)).ToArray();

      Assert.That(select.Length, Is.EqualTo(10));
    }
  }
}
