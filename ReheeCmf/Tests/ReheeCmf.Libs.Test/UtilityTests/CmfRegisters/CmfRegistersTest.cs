using NUnit.Framework;
using ReheeCmf.Components;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Utility.CmfRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.UtilityTests.CmfRegisters
{
  internal class CmfRegistersTest
  {
    [SetUp]
    public virtual void Setup()
    {

    }

    [Test]
    public void InitRegister()
    {
      CmfRegister.Init();
      var pool = CmfRegister.ComponentPool.Values.ToArray();
      var registers = CmfRegister.ComponentPool.Values.Where(b => b is ITestComponent).ToArray();

      Assert.That(registers.Length, Is.EqualTo(3));
    }




  }
  [TestRegister<TestUsedClass, TestHandler>]
  file class TestUsedClass
  {

  }
  [TestRegister<TestUsedClass, TestHandler>]
  file class TestUsedClass2
  {

  }
  [TestRegister2<TestUsedClass3, TestHandler>]
  file class TestUsedClass3
  {

  }
  [TestRegister2<TestUsedClass4, TestHandler2>]
  file class TestUsedClass4
  {

  }
  file interface ITestComponent
  {

  }
  file class TestRegister2Attribute<K, T> : TestRegisterAttribute<K, T> where T : ICmfHandler, new()
  {

  }
  file class TestRegisterAttribute<K, T> : CmfComponentAttribute, IEntityComponent, ITestComponent where T : ICmfHandler, new()
  {
    public override Type? EntityType => typeof(K);
    public override Type? HandlerType => typeof(T);
  }

  file class TestHandler : ICmfHandler
  {

  }
  file class TestHandler2 : ICmfHandler
  {

  }
}
