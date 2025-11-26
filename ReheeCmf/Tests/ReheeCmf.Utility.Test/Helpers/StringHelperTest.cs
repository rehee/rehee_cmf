namespace ReheeCmf.Utility.Test.Helpers
{
  public class StringHelperTest
  {
    [Test]
    public void SplitPascalCase_WithPascalCase_ReturnsSpacedString()
    {
      var result = "HelloWorld".SplitPascalCase();
      Assert.That(result, Is.EqualTo("Hello World"));
    }

    [Test]
    public void SplitPascalCase_WithAcronym_ReturnsSplitAcronym()
    {
      var result = "XMLParser".SplitPascalCase();
      Assert.That(result, Is.EqualTo("XML Parser"));
    }

    [Test]
    public void SplitPascalCase_WithSingleWord_ReturnsSameWord()
    {
      var result = "Hello".SplitPascalCase();
      Assert.That(result, Is.EqualTo("Hello"));
    }

    [Test]
    public void GetSystemType_WithBooleanTypeCode_ReturnsBoolType()
    {
      var result = TypeCode.Boolean.GetSystemType();
      Assert.That(result, Is.EqualTo(typeof(bool)));
    }

    [Test]
    public void GetSystemType_WithInt32TypeCode_ReturnsInt32Type()
    {
      var result = TypeCode.Int32.GetSystemType();
      Assert.That(result, Is.EqualTo(typeof(Int32)));
    }

    [Test]
    public void GetSystemType_WithStringTypeCode_ReturnsStringType()
    {
      var result = TypeCode.String.GetSystemType();
      Assert.That(result, Is.EqualTo(typeof(string)));
    }

    [Test]
    public void GetSystemType_WithDateTimeTypeCode_ReturnsDateTimeType()
    {
      var result = TypeCode.DateTime.GetSystemType();
      Assert.That(result, Is.EqualTo(typeof(DateTime)));
    }

    [Test]
    public void GetSystemType_WithDecimalTypeCode_ReturnsDecimalType()
    {
      var result = TypeCode.Decimal.GetSystemType();
      Assert.That(result, Is.EqualTo(typeof(decimal)));
    }
  }
}
