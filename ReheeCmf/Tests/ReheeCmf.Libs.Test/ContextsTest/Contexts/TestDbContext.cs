using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.ContextsTest.Contexts
{
  internal class TestDbContext : CmfIdentityContext<ReheeCmfBaseUser>
  {
    public TestDbContext(IServiceProvider sp) : base(sp)
    {
    }
    public DbSet<TestEntity> TestEntities { get; set; }
    public DbSet<TestEntity2> TestEntity2s { get; set; }
    public DbSet<TestEntity3> TestEntity3s { get; set; }
    public DbSet<TestValidationEntity> TestValidationEntities { get; set; }
  }
  internal class TestDbContext2 : CmfDbContext
  {
    public TestDbContext2(IServiceProvider sp) : base(sp)
    {
    }
    public DbSet<TestEntity> TestEntities { get; set; }
    public DbSet<TestEntity2> TestEntity2s { get; set; }
    public DbSet<TestEntity3> TestEntity3s { get; set; }
    public DbSet<TestValidationEntity> TestValidationEntities { get; set; }
  }
}
