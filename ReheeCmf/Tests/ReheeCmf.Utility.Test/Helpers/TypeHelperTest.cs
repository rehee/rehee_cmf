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
  }
}
