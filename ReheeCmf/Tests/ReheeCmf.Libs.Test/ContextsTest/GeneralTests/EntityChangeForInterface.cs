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


  internal interface ITest
  {

  }
  [EntityChangeTracker<TestEntity, TestInterfaceHandler>]
  internal class TestInterfaceHandler : EntityChangeHandler<TestEntity>
  {

  }
}
