using NUnit.Framework;
using ReheeCmf.Components;
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
      var registers = CmfRegister.ComponentPool.Values.Where(b => b is ITestComponent).ToArray();

      Assert.That(registers.Length, Is.EqualTo(3));
    }




  }
  [TestRegister<TestHandler>]
  file class TestUsedClass
  {

  }
  [TestRegister<TestHandler>]
  file class TestUsedClass2
  {

  }
  [TestRegister2<TestHandler>]
  file class TestUsedClass3
  {

  }
  [TestRegister2<TestHandler2>]
  file class TestUsedClass4
  {

  }
  file interface ITestComponent
  {

  }
  file class TestRegister2Attribute<T> : TestRegisterAttribute<T> where T : ICmfHandler, new()
  {

  }
  file class TestRegisterAttribute<T> : CmfComponentAttribute<T>, ITestComponent where T : ICmfHandler, new()
  {

  }

  file class TestHandler : ICmfHandler
  {

  }
  file class TestHandler2 : ICmfHandler
  {

  }
}
