namespace ReheeCmf.Utility.Test.Helpers
{
  public class TypeHelperTest
  {
    [Test]
    public void IsIEnumerable_WithString_NoString_ReturnsFalse()
    {
      var result = typeof(string).IsIEnumerable(noString: true);
      Assert.That(result, Is.False);
    }

    [Test]
    public void IsIEnumerable_WithString_AllowString_ReturnsTrue()
    {
      var result = typeof(string).IsIEnumerable(noString: false);
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsIEnumerable_WithList_ReturnsTrue()
    {
      var result = typeof(List<int>).IsIEnumerable();
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsIEnumerable_WithInt_ReturnsFalse()
    {
      var result = typeof(int).IsIEnumerable();
      Assert.That(result, Is.False);
    }

    [Test]
    public void IsNullable_WithNullableInt_ReturnsTrue()
    {
      var result = typeof(int?).IsNullable();
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsNullable_WithInt_ReturnsFalse()
    {
      var result = typeof(int).IsNullable();
      Assert.That(result, Is.False);
    }

    [Test]
    public void IsImplement_WithInterface_ReturnsTrue()
    {
      var result = typeof(List<int>).IsImplement(typeof(System.Collections.IEnumerable));
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsImplement_WithNotImplementedInterface_ReturnsFalse()
    {
      var result = typeof(int).IsImplement(typeof(System.Collections.IEnumerable));
      Assert.That(result, Is.False);
    }

    [Test]
    public void IsImplement_Generic_WithInterface_ReturnsTrue()
    {
      var result = typeof(List<int>).IsImplement<System.Collections.IEnumerable>();
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsSimpleType_WithInt_ReturnsTrue()
    {
      var result = typeof(int).IsSimpleType();
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsSimpleType_WithObject_ReturnsFalse()
    {
      var result = typeof(object).IsSimpleType();
      Assert.That(result, Is.False);
    }

    [Test]
    public void IsInheritance_WithSameType_ReturnsTrue()
    {
      var result = typeof(string).IsInheritance(typeof(string));
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsInheritance_WithBaseType_ReturnsTrue()
    {
      var result = typeof(string).IsInheritance(typeof(object));
      Assert.That(result, Is.True);
    }

    [Test]
    public void IsInheritance_WithUnrelatedType_ReturnsFalse()
    {
      var result = typeof(string).IsInheritance(typeof(int));
      Assert.That(result, Is.False);
    }

    // Tests for HasAttribute<TAttribute>
    [Test]
    public void HasAttribute_Generic_WithExistingAttribute_ReturnsTrue()
    {
      var result = typeof(TestClassWithAttribute).HasAttribute<SerializableAttribute>();
      Assert.That(result, Is.True);
    }

    [Test]
    public void HasAttribute_Generic_WithNoAttribute_ReturnsFalse()
    {
      var result = typeof(TestClassWithoutAttribute).HasAttribute<SerializableAttribute>();
      Assert.That(result, Is.False);
    }

    [Test]
    public void HasAttribute_Generic_WithNullType_ReturnsFalse()
    {
      Type? nullType = null;
      var result = nullType!.HasAttribute<SerializableAttribute>();
      Assert.That(result, Is.False);
    }

    // Tests for HasAttribute(Type attributeType)
    [Test]
    public void HasAttribute_WithExistingAttribute_ReturnsTrue()
    {
      var result = typeof(TestClassWithAttribute).HasAttribute(typeof(SerializableAttribute));
      Assert.That(result, Is.True);
    }

    [Test]
    public void HasAttribute_WithNoAttribute_ReturnsFalse()
    {
      var result = typeof(TestClassWithoutAttribute).HasAttribute(typeof(SerializableAttribute));
      Assert.That(result, Is.False);
    }

    [Test]
    public void HasAttribute_WithNullType_ReturnsFalse()
    {
      Type? nullType = null;
      var result = nullType!.HasAttribute(typeof(SerializableAttribute));
      Assert.That(result, Is.False);
    }

    [Test]
    public void HasAttribute_WithNullAttributeType_ReturnsFalse()
    {
      var result = typeof(TestClassWithAttribute).HasAttribute(null!);
      Assert.That(result, Is.False);
    }

    [Test]
    public void HasAttribute_WithNonAttributeType_ReturnsFalse()
    {
      var result = typeof(TestClassWithAttribute).HasAttribute(typeof(string));
      Assert.That(result, Is.False);
    }

    // Tests for ImplementsInterface
    [Test]
    public void ImplementsInterface_WithImplementedInterface_ReturnsTrue()
    {
      var result = typeof(List<int>).ImplementsInterface(typeof(IEnumerable<int>));
      Assert.That(result, Is.True);
    }

    [Test]
    public void ImplementsInterface_WithNonImplementedInterface_ReturnsFalse()
    {
      var result = typeof(string).ImplementsInterface(typeof(IDisposable));
      Assert.That(result, Is.False);
    }

    [Test]
    public void ImplementsInterface_WithNullType_ReturnsFalse()
    {
      Type? nullType = null;
      var result = nullType!.ImplementsInterface(typeof(IDisposable));
      Assert.That(result, Is.False);
    }

    [Test]
    public void ImplementsInterface_WithNullInterfaceType_ReturnsFalse()
    {
      var result = typeof(string).ImplementsInterface(null!);
      Assert.That(result, Is.False);
    }

    [Test]
    public void ImplementsInterface_WithNonInterfaceType_ReturnsFalse()
    {
      var result = typeof(string).ImplementsInterface(typeof(object));
      Assert.That(result, Is.False);
    }

    [Test]
    public void ImplementsInterface_WithGenericInterface_ReturnsTrue()
    {
      var result = typeof(List<string>).ImplementsInterface(typeof(IList<>));
      Assert.That(result, Is.True);
    }

    [Test]
    public void ImplementsInterface_Generic_WithImplementedInterface_ReturnsTrue()
    {
      var result = TypeHelper.ImplementsInterface<List<int>>(typeof(IEnumerable<int>));
      Assert.That(result, Is.True);
    }

    // Tests for InheritsFrom
    [Test]
    public void InheritsFrom_WithBaseClass_ReturnsTrue()
    {
      var result = typeof(DerivedTestClass).InheritsFrom(typeof(BaseTestClass));
      Assert.That(result, Is.True);
    }

    [Test]
    public void InheritsFrom_WithNonBaseClass_ReturnsFalse()
    {
      var result = typeof(string).InheritsFrom(typeof(List<int>));
      Assert.That(result, Is.False);
    }

    [Test]
    public void InheritsFrom_WithInterface_ReturnsTrue()
    {
      var result = typeof(List<int>).InheritsFrom(typeof(IEnumerable<int>));
      Assert.That(result, Is.True);
    }

    [Test]
    public void InheritsFrom_WithNullType_ReturnsFalse()
    {
      Type? nullType = null;
      var result = nullType!.InheritsFrom(typeof(object));
      Assert.That(result, Is.False);
    }

    [Test]
    public void InheritsFrom_WithNullBaseType_ReturnsFalse()
    {
      var result = typeof(string).InheritsFrom(null!);
      Assert.That(result, Is.False);
    }

    [Test]
    public void InheritsFrom_Generic_WithBaseClass_ReturnsTrue()
    {
      var result = typeof(DerivedTestClass).InheritsFrom<BaseTestClass>();
      Assert.That(result, Is.True);
    }

    [Test]
    public void InheritsFrom_WithGenericBaseClass_ReturnsTrue()
    {
      var result = typeof(DerivedGenericTestClass).InheritsFrom(typeof(GenericBaseTestClass<>));
      Assert.That(result, Is.True);
    }
  }

  // Test classes for attribute tests
  [Serializable]
  public class TestClassWithAttribute { }

  public class TestClassWithoutAttribute { }

  // Test classes for inheritance tests
  public class BaseTestClass { }

  public class DerivedTestClass : BaseTestClass { }

  public class GenericBaseTestClass<T> { }

  public class DerivedGenericTestClass : GenericBaseTestClass<int> { }
}
