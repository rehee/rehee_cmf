using System.Reflection;

namespace ReheeCmf.Utility.Test.Helpers
{
  public class AttributeHelperTest
  {
    [Test]
    public void GetTypedAttribute_WithExistingAttribute_ReturnsAttribute()
    {
      var result = typeof(TestClassWithAttribute).GetTypedAttribute<TestCustomAttribute>();
      Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetTypedAttribute_WithNoAttribute_ReturnsNull()
    {
      var result = typeof(TestClassWithoutAttribute).GetTypedAttribute<TestCustomAttribute>();
      Assert.That(result, Is.Null);
    }

    [Test]
    public void HasCustomAttribute_Type_WithExistingAttribute_ReturnsTrue()
    {
      var result = typeof(TestClassWithAttribute).HasCustomAttribute<TestCustomAttribute>();
      Assert.That(result, Is.True);
    }

    [Test]
    public void HasCustomAttribute_Type_WithNoAttribute_ReturnsFalse()
    {
      var result = typeof(TestClassWithoutAttribute).HasCustomAttribute<TestCustomAttribute>();
      Assert.That(result, Is.False);
    }

    [Test]
    public void HasCustomAttribute_Type_ByName_WithExistingAttribute_ReturnsTrue()
    {
      var result = typeof(TestClassWithAttribute).HasCustomAttribute("TestCustomAttribute");
      Assert.That(result, Is.True);
    }

    [Test]
    public void HasCustomAttribute_Property_WithExistingAttribute_ReturnsTrue()
    {
      var property = typeof(TestClassWithPropertyAttribute).GetProperty("TestProperty")!;
      var result = property.HasCustomAttribute<TestCustomAttribute>();
      Assert.That(result, Is.True);
    }

    [Test]
    public void HasCustomAttribute_Property_WithNoAttribute_ReturnsFalse()
    {
      var property = typeof(TestClassWithPropertyAttribute).GetProperty("PropertyWithoutAttribute")!;
      var result = property.HasCustomAttribute<TestCustomAttribute>();
      Assert.That(result, Is.False);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    private class TestCustomAttribute : Attribute { }

    [TestCustom]
    private class TestClassWithAttribute { }

    private class TestClassWithoutAttribute { }

    private class TestClassWithPropertyAttribute
    {
      [TestCustom]
      public string? TestProperty { get; set; }
      public string? PropertyWithoutAttribute { get; set; }
    }
  }
}
