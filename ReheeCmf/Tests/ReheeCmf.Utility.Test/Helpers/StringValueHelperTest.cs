using ReheeCmf.Helpers;
using ReheeCmf.Responses;

namespace ReheeCmf.Utility.Test.Helpers
{
  public class StringValueHelperTest
  {
    // Tests for GetObjValue<T>
    [Test]
    public void GetObjValue_Generic_WithValidBool_ReturnsSuccessfulResponse()
    {
      var result = "true".GetObjValue<bool>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(true));
    }

    [Test]
    public void GetObjValue_Generic_WithValidInt_ReturnsSuccessfulResponse()
    {
      var result = "42".GetObjValue<int>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(42));
    }

    [Test]
    public void GetObjValue_Generic_WithInvalidInt_ReturnsUnsuccessfulResponse()
    {
      var result = "not a number".GetObjValue<int>();

      Assert.That(result.Success, Is.False);
    }

    [Test]
    public void GetObjValue_Generic_WithNullableInt_AndNullInput_ReturnsUnsuccessful()
    {
      // Note: The generic GetObjValue<T> doesn't handle nullable value types with null input
      // because the underlying result.Content is null which doesn't match "is T tValue" check
      var result = ((string?)null).GetObjValue<int?>();

      Assert.That(result.Success, Is.False);
    }

    [Test]
    public void GetObjValue_Generic_WithValidGuid_ReturnsSuccessfulResponse()
    {
      var guid = Guid.NewGuid();
      var result = guid.ToString().GetObjValue<Guid>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(guid));
    }

    [Test]
    public void GetObjValue_Generic_WithValidString_ReturnsSuccessfulResponse()
    {
      var result = "hello world".GetObjValue<string>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo("hello world"));
    }

    // Tests for GetValue<T>
    [Test]
    public void GetValue_WithValidInt_ReturnsIntValue()
    {
      var result = "123".GetValue<int>();

      Assert.That(result, Is.EqualTo(123));
    }

    [Test]
    public void GetValue_WithInvalidInt_ReturnsDefault()
    {
      var result = "invalid".GetValue<int>();

      Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void GetValue_WithValidBool_ReturnsBoolValue()
    {
      var result = "false".GetValue<bool>();

      Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void GetValue_WithValidDouble_ReturnsDoubleValue()
    {
      var result = "3.14".GetValue<double>();

      Assert.That(result, Is.EqualTo(3.14));
    }

    [Test]
    public void GetValue_WithValidDecimal_ReturnsDecimalValue()
    {
      var result = "99.99".GetValue<decimal>();

      Assert.That(result, Is.EqualTo(99.99m));
    }

    // Tests for StringValue
    [Test]
    public void StringValue_WithNull_ReturnsNull()
    {
      object? input = null;
      var result = input.StringValue();

      Assert.That(result, Is.Null);
    }

    [Test]
    public void StringValue_WithInt_ReturnsStringRepresentation()
    {
      object input = 42;
      var result = input.StringValue();

      Assert.That(result, Is.EqualTo("42"));
    }

    [Test]
    public void StringValue_WithBool_ReturnsStringRepresentation()
    {
      object input = true;
      var result = input.StringValue();

      Assert.That(result, Is.EqualTo("True"));
    }

    [Test]
    public void StringValue_WithGuid_ReturnsStringRepresentation()
    {
      var guid = Guid.NewGuid();
      object input = guid;
      var result = input.StringValue();

      Assert.That(result, Is.EqualTo(guid.ToString()));
    }

    // Tests for type codes
    [Test]
    public void GetObjValue_WithChar_ReturnsCharValue()
    {
      var result = "A".GetObjValue<char>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo('A'));
    }

    [Test]
    public void GetObjValue_WithByte_ReturnsByteValue()
    {
      var result = "255".GetObjValue<byte>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo((byte)255));
    }

    [Test]
    public void GetObjValue_WithSByte_ReturnsSByteValue()
    {
      var result = "-128".GetObjValue<sbyte>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo((sbyte)-128));
    }

    [Test]
    public void GetObjValue_WithInt16_ReturnsInt16Value()
    {
      var result = "32767".GetObjValue<short>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo((short)32767));
    }

    [Test]
    public void GetObjValue_WithUInt16_ReturnsUInt16Value()
    {
      var result = "65535".GetObjValue<ushort>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo((ushort)65535));
    }

    [Test]
    public void GetObjValue_WithInt64_ReturnsInt64Value()
    {
      var result = "9223372036854775807".GetObjValue<long>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void GetObjValue_WithUInt32_ReturnsUInt32Value()
    {
      var result = "4294967295".GetObjValue<uint>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(uint.MaxValue));
    }

    [Test]
    public void GetObjValue_WithUInt64_ReturnsUInt64Value()
    {
      var result = "18446744073709551615".GetObjValue<ulong>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(ulong.MaxValue));
    }

    [Test]
    public void GetObjValue_WithSingle_ReturnsSingleValue()
    {
      var result = "3.14".GetObjValue<float>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(3.14f).Within(0.001f));
    }

    // Tests for nullable types
    [Test]
    public void GetObjValue_WithNullableBool_AndInvalidValue_ReturnsUnsuccessful()
    {
      // Note: The generic GetObjValue<T> doesn't handle nullable value types with null content
      var result = "invalid".GetObjValue<bool?>();

      Assert.That(result.Success, Is.False);
    }

    [Test]
    public void GetObjValue_WithNullableGuid_AndEmptyString_ReturnsUnsuccessful()
    {
      // Note: The generic GetObjValue<T> doesn't handle nullable value types with null content
      var result = "".GetObjValue<Guid?>();

      Assert.That(result.Success, Is.False);
    }

    // Tests for enum
    [Test]
    public void GetObjValue_WithValidEnum_ReturnsEnumValue()
    {
      var result = "Value1".GetObjValue<TestEnumForStringValue>();

      Assert.That(result.Success, Is.True);
      Assert.That(result.Content, Is.EqualTo(TestEnumForStringValue.Value1));
    }

    [Test]
    public void GetObjValue_WithInvalidEnum_ReturnsUnsuccessful()
    {
      var result = "InvalidValue".GetObjValue<TestEnumForStringValue>();

      Assert.That(result.Success, Is.False);
    }
  }

  // Test enum for StringValueHelper tests
  public enum TestEnumForStringValue
  {
    NotSpecified = 0,
    Value1 = 1,
    Value2 = 2
  }
}
