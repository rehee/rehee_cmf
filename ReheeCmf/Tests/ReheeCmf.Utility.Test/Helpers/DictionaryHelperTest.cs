namespace ReheeCmf.Utility.Test.Helpers
{
  public class DictionaryHelperTest
  {
    [Test]
    public void TryGetValueStringKey_WithExactMatch_ReturnsTrue()
    {
      var dict = new Dictionary<string, string?>
      {
        ["Key1"] = "Value1",
        ["Key2"] = "Value2"
      };
      var result = dict.TryGetValueStringKey("Key1", out var value);
      Assert.That(result, Is.True);
      Assert.That(value, Is.EqualTo("Value1"));
    }

    [Test]
    public void TryGetValueStringKey_WithCaseInsensitiveMatch_ReturnsTrue()
    {
      var dict = new Dictionary<string, string?>
      {
        ["Key1"] = "Value1",
        ["Key2"] = "Value2"
      };
      var result = dict.TryGetValueStringKey("KEY1", out var value);
      Assert.That(result, Is.True);
      Assert.That(value, Is.EqualTo("Value1"));
    }

    [Test]
    public void TryGetValueStringKey_WithNoMatch_ReturnsFalse()
    {
      var dict = new Dictionary<string, string?>
      {
        ["Key1"] = "Value1"
      };
      var result = dict.TryGetValueStringKey("Key2", out var value);
      Assert.That(result, Is.False);
      Assert.That(value, Is.Null);
    }

    [Test]
    public void TryGetValueStringKey_WithNullDictionary_ReturnsFalse()
    {
      Dictionary<string, string?>? dict = null;
      var result = dict.TryGetValueStringKey("Key1", out var value);
      Assert.That(result, Is.False);
      Assert.That(value, Is.Null);
    }

    [Test]
    public void ToDictionWithKeys_WithMatchingKeys_ReturnsFilteredDictionary()
    {
      var dict = new Dictionary<string, string?>
      {
        ["Key1"] = "Value1",
        ["Key2"] = "Value2",
        ["Key3"] = "Value3"
      };
      var keys = new[] { "Key1", "Key3" };
      var result = dict.ToDictionWithKeys(keys);
      Assert.That(result.Count, Is.EqualTo(2));
      Assert.That(result.ContainsKey("Key1"), Is.True);
      Assert.That(result.ContainsKey("Key3"), Is.True);
    }

    [Test]
    public void ToDictionWithKeys_WithCaseInsensitiveKeys_ReturnsFilteredDictionary()
    {
      var dict = new Dictionary<string, string?>
      {
        ["Key1"] = "Value1",
        ["Key2"] = "Value2"
      };
      var keys = new[] { "KEY1" };
      var result = dict.ToDictionWithKeys(keys);
      Assert.That(result.Count, Is.EqualTo(1));
      Assert.That(result.ContainsKey("Key1"), Is.True);
    }
  }
}
