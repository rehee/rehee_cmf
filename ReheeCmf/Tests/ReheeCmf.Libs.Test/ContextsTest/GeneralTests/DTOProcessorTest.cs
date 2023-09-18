using ReheeCmf.Commons.DTOs;
using ReheeCmf.DTOProcessors;
using ReheeCmf.DTOProcessors.Processors;
using ReheeCmf.Handlers.InterfaceChangeHandlers;
using ReheeCmf.Services;
using System.Runtime.InteropServices;

namespace ReheeCmf.Libs.Test.ContextsTest.GeneralTests
{
  internal class DTOProcessorTest1 : DTOProcessorTest<TestDbContext>
  {
  }
  internal class DTOProcessorTest2 : DTOProcessorTest<TestDbContext2>
  {
  }
  internal abstract class DTOProcessorTest<TDbContext> : ContextsTest<TDbContext> where TDbContext : DbContext
  {
    private IServiceProvider sp { get; set; }

    [SetUp]
    public override void Setup()
    {
      base.Setup();
      sp = ConfigService(
        service => service.AddScoped<TestDTOProcessor, TestDTOProcessor>()
        );
    }

    [Test]
    public void UpdateJsonTest_Null_PropertyNameWIllNotUpdate()
    {
      var db = sp.GetService<IContext>();
      var processor = sp.GetService<TestDTOProcessor>();
      var name = "Test";
      var entity1 = new TestEntity
      {
        Name = name,
      };

      db!.AddAsync<TestEntity>(entity1, CancellationToken.None).Wait();
      db.SaveChanges(null);
      processor.InitializationAsync("1", "{}", null, CancellationToken.None).Wait();

      Assert.That(processor!.dto!.Name1!, Is.EqualTo(name));
    }
    [Test]
    public void UpdateJsonTest_Null_PropertyNameWIllUpdate()
    {
      var db = sp.GetService<IContext>();
      var processor = sp.GetService<TestDTOProcessor>();
      var name = "Test";
      var entity1 = new TestEntity
      {
        Name = name,
      };

      db!.AddAsync<TestEntity>(entity1, CancellationToken.None).Wait();
      db.SaveChanges(null);
      processor.InitializationAsync("1", """{"name1":""}""", null, CancellationToken.None).Wait();

      Assert.That(processor!.dto!.Name1!, Is.EqualTo(""));
    }
  }

  internal class TestDTOProcessor : DtoProcessor<TestEntity1DTO>
  {
    public TestDTOProcessor(IContext context, IAsyncQuery asyncQuery) : base(context, asyncQuery)
    {
    }
    public override IQueryable<TestEntity1DTO> QueryWithType(TokenDTO user)
    {
      return context.Query<TestEntity>(true).Select(b =>
      new TestEntity1DTO
      {
        QueryKey = b.Id.ToString(),
        Name1 = b.Name,
        Name2 = b.Before_Name,
        Name3 = b.After_Name,
      });
    }
  }

  internal class TestEntity1DTO : CmfDTOBase<int>
  {
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    public string? Name3 { get; set; }
  }
}
