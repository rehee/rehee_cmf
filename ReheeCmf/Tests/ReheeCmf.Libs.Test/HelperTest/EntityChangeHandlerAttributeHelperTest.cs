using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.HelperTest
{
  public class EntityChangeHandlerAttributeHelperTest
  {
    [Test]
    public void EntityChangeHandlerAttributeHelperTest_GetHandler()
    {
      var type = typeof(TestClass).GetTypedAttribute<EntityChangeHandlerAttribute>();
      Assert.IsNull(type);
      var type2 = typeof(TestClass2).GetTypedAttribute<EntityChangeHandlerAttribute>();
      Assert.IsNull(type2);
      var type1 = typeof(TestClass1).GetTypedAttribute<EntityChangeHandlerAttribute>();
      Assert.NotNull(type1);
      var type3 = typeof(TestClass3).GetTypedAttribute<EntityChangeHandlerAttribute>();
      Assert.NotNull(type3);
    }
  }
  file class TestClass
  {

  }
  [EntityChangeHandler<Handler>]
  file class TestClass1
  {

  }
  file class TestClass2
  {

  }
  [EntityChangeHandler<Handler>]
  file class TestClass3
  {

  }
  file class Handler : EntityChangeHandler<TestClass>
  {
  }
}
