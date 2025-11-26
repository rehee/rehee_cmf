namespace ReheeCmf.Utility.Test.Helpers
{
  public class EnumIdTypeHelperTest
  {
    [Test]
    public void GetIdType_WithGuid_ReturnsGuid()
    {
      var result = EnumIdTypeHelper.GetIdType<Guid>();
      Assert.That(result, Is.EqualTo(EnumIdType.Guid));
    }

    [Test]
    public void GetIdType_WithNullableGuid_ReturnsGuid()
    {
      var result = EnumIdTypeHelper.GetIdType<Guid?>();
      Assert.That(result, Is.EqualTo(EnumIdType.Guid));
    }

    [Test]
    public void GetIdType_WithString_ReturnsString()
    {
      var result = EnumIdTypeHelper.GetIdType<string>();
      Assert.That(result, Is.EqualTo(EnumIdType.String));
    }

    [Test]
    public void GetIdType_WithInt32_ReturnsNumber()
    {
      var result = EnumIdTypeHelper.GetIdType<Int32>();
      Assert.That(result, Is.EqualTo(EnumIdType.Number));
    }

    [Test]
    public void GetIdType_WithLong_ReturnsNumber()
    {
      var result = EnumIdTypeHelper.GetIdType<long>();
      Assert.That(result, Is.EqualTo(EnumIdType.Number));
    }

    [Test]
    public void GetIdType_WithUnknownType_ReturnsNotSpecified()
    {
      var result = EnumIdTypeHelper.GetIdType<double>();
      Assert.That(result, Is.EqualTo(EnumIdType.NotSpecified));
    }
  }
}
