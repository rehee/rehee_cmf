using System.ComponentModel.DataAnnotations;
using ReheeCmf.Helpers;

namespace ReheeCmf.Utility.Test.Helpers
{
  public class CommonHelperTest
  {
    [Test]
    public void CreateStringObj_WithKeyAndValue_ReturnsKeyValuePair()
    {
      var result = CommonHelper.CreateStringObj("testKey", "testValue");

      Assert.That(result.Key, Is.EqualTo("testKey"));
      Assert.That(result.Value, Is.EqualTo("testValue"));
    }

    [Test]
    public void CreateStringObj_WithNullValue_ReturnsKeyValuePairWithNullValue()
    {
      var result = CommonHelper.CreateStringObj("testKey", null!);

      Assert.That(result.Key, Is.EqualTo("testKey"));
      Assert.That(result.Value, Is.Null);
    }

    [Test]
    public void CreateStringObj_WithIntegerValue_ReturnsKeyValuePairWithIntValue()
    {
      var result = CommonHelper.CreateStringObj("count", 42);

      Assert.That(result.Key, Is.EqualTo("count"));
      Assert.That(result.Value, Is.EqualTo(42));
    }
  }

  public class KeyValuePairFuncTest
  {
    [Test]
    public void CreateStringObj_WithKeyAndValue_ReturnsKeyValuePair()
    {
      var result = KeyValuePairFunc.CreateStringObj("testKey", "testValue");

      Assert.That(result.Key, Is.EqualTo("testKey"));
      Assert.That(result.Value, Is.EqualTo("testValue"));
    }
  }
}
