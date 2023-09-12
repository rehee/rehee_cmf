using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReheeCmf.Components.ChangeComponents;

namespace ReheeCmf.Libs.Test.HelperTest
{
    public class EntityChangeHandlerAttributeHelperTest
  {
    [Test]
    public void EntityChangeHandlerAttributeHelperTest_GetHandler()
    {
      var type = typeof(TestClass).GetComponentByType<IEntityChangeComponent>().FirstOrDefault();
      Assert.IsNull(type);
      var type2 = typeof(TestClass2).GetComponentByType<IEntityChangeComponent>().FirstOrDefault();
      Assert.IsNull(type2);
      var type1 = typeof(TestClass1).GetComponentByType<IEntityChangeComponent>().FirstOrDefault();
      Assert.NotNull(type1);
      var type3 = typeof(TestClass3).GetComponentByType<IEntityChangeComponent>().FirstOrDefault();
      Assert.NotNull(type3);
    }
  }
  file class TestClass
  {

  }
  [EntityChange<Handler>]
  file class TestClass1
  {

  }
  file class TestClass2
  {

  }
  [EntityChange<Handler>]
  file class TestClass3
  {

  }
  file class Handler : EntityChangeHandler<TestClass>
  {
  }
}
