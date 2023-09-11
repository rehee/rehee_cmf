using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.ConstValues;
using ReheeCmf.Contexts;
using ReheeCmf.Enums;
using ReheeCmf.Libs.Test.ContextsTest;
using ReheeCmf.Libs.Test.ContextsTest.Contexts;
using ReheeCmf.Libs.Test.ContextsTest.GeneralTests;
using ReheeCmf.Reflects.ReflectPools;
using ReheeCmf.StandardInputs.Properties;
using ReheeCmf.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.Modules.ContextModuleTests
{
  internal class TestDbContextTest : ContextModuleTest<TestDbContext>
  {
  }
  internal class TestDbContext2Test : ContextModuleTest<TestDbContext2>
  {
  }
  internal abstract class ContextModuleTest<T> : ContextModuleTestBase<T> where T : DbContext
  {
    [Test]
    public async Task Context_Module_Permissions()
    {
      var permissions = await this.CmfContextModule.GetPermissions(null, null, CancellationToken.None);

      Assert.That(permissions.Where(b => b.StartsWith($"{nameof(TestEntity)}{ConstCrud.Split}")).Count(), Is.EqualTo(4));
      Assert.That(permissions.Where(b => b.StartsWith($"{nameof(TestEntity2)}{ConstCrud.Split}")).Count(), Is.EqualTo(4));
      Assert.That(permissions.Where(b => b.StartsWith($"{nameof(TestEntity3)}{ConstCrud.Split}")).Count(), Is.EqualTo(4));
      Assert.That(permissions.Where(b => b.StartsWith($"{nameof(RoleBasedPermission)}{ConstCrud.Split}")).Count(), Is.EqualTo(4));
      Assert.That(permissions.Where(b => b.StartsWith($"{nameof(TenantEntity)}{ConstCrud.Split}")).Count(), Is.EqualTo(4));
    }
    [Test]
    public async Task Context_Module_Permissions_Create()
    {
      using var db = ServiceProvider.GetService<IContext>();
      using var db2 = ServiceProvider.GetService<T>();
      db2.Database.EnsureCreated();
      var roleName = "role";
      var permisionName1 = "p1";
      var permisionName2 = "p2";
      var permisionName3 = "p3";
      await this.CmfContextModule.UpdateRoleBasedPermissionAsync(db, roleName, new Commons.DTOs.RoleBasedPermissionDTO
      {
        Items = new StandardProperty[]
        {
          new StandardProperty
          {
            PropertyName = permisionName1,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName2,
            Value="",
          },
          new StandardProperty
          {
            PropertyName = permisionName3,
            Value="True",
          }
        }
      }, null);
      var moduleName = CmfContextModule.ModuleName.ToUpper();
      var roleNameUp = roleName.ToUpper();
      var records = db.Query<RoleBasedPermission>(true)
        .Where(b =>
          b.NormalizationModuleName == moduleName &&
          b.NormalizationRoleName == roleNameUp
          )
        .ToList();
      Assert.That(records.Count, Is.EqualTo(1));
      var r = records.FirstOrDefault();
      Assert.That(r.PermissionList.Count, Is.EqualTo(0));
    }
    [Test]
    public async Task Context_Module_Permissions_with_entity_Create()
    {
      using var db = ServiceProvider.GetService<IContext>();
      using var db2 = ServiceProvider.GetService<T>();
      db2.Database.EnsureCreated();
      var roleName = "role";
      var permisionName1 = EnumHttpMethod.Get.GetEntityPermission(nameof(TestEntity));
      var permisionName2 = "p2";
      var permisionName3 = "p3";
      await this.CmfContextModule.UpdateRoleBasedPermissionAsync(db, roleName, new Commons.DTOs.RoleBasedPermissionDTO
      {
        Items = new StandardProperty[]
        {
          new StandardProperty
          {
            PropertyName = permisionName1,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName2,
            Value="",
          },
          new StandardProperty
          {
            PropertyName = permisionName3,
            Value="True",
          }
        }
      }, null);
      var moduleName = CmfContextModule.ModuleName.ToUpper();
      var roleNameUp = roleName.ToUpper();
      var records = db.Query<RoleBasedPermission>(true)
        .Where(b =>
          b.NormalizationModuleName == moduleName &&
          b.NormalizationRoleName == roleNameUp
          )
        .ToList();
      Assert.That(records.Count, Is.EqualTo(1));
      var r = records.FirstOrDefault();
      Assert.That(r.PermissionList.Count, Is.EqualTo(1));
    }
    [Test]
    public async Task Context_Module_Permissions_with_entity_Create_Update()
    {
      using var db = ServiceProvider.GetService<IContext>();
      using var db2 = ServiceProvider.GetService<T>();
      db2.Database.EnsureCreated();
      var roleName = "role";
      var permisionName1 = EnumHttpMethod.Get.GetEntityPermission(nameof(TestEntity));
      var permisionName2 = EnumHttpMethod.Post.GetEntityPermission(nameof(TestEntity));
      var permisionName3 = "p3";
      await this.CmfContextModule.UpdateRoleBasedPermissionAsync(db, roleName, new Commons.DTOs.RoleBasedPermissionDTO
      {
        Items = new StandardProperty[]
        {
          new StandardProperty
          {
            PropertyName = permisionName1,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName2,
            Value="",
          },
          new StandardProperty
          {
            PropertyName = permisionName3,
            Value="True",
          }
        }
      }, null);
      var moduleName = CmfContextModule.ModuleName.ToUpper();
      var roleNameUp = roleName.ToUpper();
      var records = db.Query<RoleBasedPermission>(true)
        .Where(b =>
          b.NormalizationModuleName == moduleName &&
          b.NormalizationRoleName == roleNameUp
          )
        .ToList();
      Assert.That(records.Count, Is.EqualTo(1));
      var r = records.FirstOrDefault();
      Assert.That(r.PermissionList.Count, Is.EqualTo(1));
      await this.CmfContextModule.UpdateRoleBasedPermissionAsync(db, roleName, new Commons.DTOs.RoleBasedPermissionDTO
      {
        Items = new StandardProperty[]
        {
          new StandardProperty
          {
            PropertyName = permisionName1,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName2,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName3,
            Value="True",
          }
        }
      }, null);
      records = db.Query<RoleBasedPermission>(true)
        .Where(b =>
          b.NormalizationModuleName == moduleName &&
          b.NormalizationRoleName == roleNameUp
          )
        .ToList();
      Assert.That(records.Count, Is.EqualTo(1));
      r = records.FirstOrDefault();
      Assert.That(r.PermissionList.Count, Is.EqualTo(2));
    }
    [Test]
    public async Task Context_Module_Permission_Item_Test()
    {
      using var db = ServiceProvider.GetService<IContext>();
      using var db2 = ServiceProvider.GetService<T>();
      db2.Database.EnsureCreated();
      var roleName = "role";
      var permisionName1 = EnumHttpMethod.Get.GetEntityPermission(nameof(TestEntity));
      var permisionName2 = "p2";
      var permisionName3 = EnumHttpMethod.Post.GetEntityPermission(nameof(TestEntity));
      await this.CmfContextModule.UpdateRoleBasedPermissionAsync(db, roleName, new Commons.DTOs.RoleBasedPermissionDTO
      {
        Items = new StandardProperty[]
        {
          new StandardProperty
          {
            PropertyName = permisionName1,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName2,
            Value="True",
          },
          new StandardProperty
          {
            PropertyName = permisionName3,
            Value="True",
          }
        }
      }, null);
      var item = await this.CmfContextModule.GetRoleBasedPermissionAsync(db, new string[] { roleName }, "");
      var strings = item.Content.Split(",").ToArray();
      Assert.That(strings.Length,Is.EqualTo(2));
      Assert.True(strings.Contains(permisionName1));
      Assert.True(strings.Contains(permisionName3));
    }
  }
}
