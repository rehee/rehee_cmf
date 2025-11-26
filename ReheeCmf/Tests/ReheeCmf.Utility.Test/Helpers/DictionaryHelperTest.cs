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

    // Tests for TryAddOrUpdate
    [Test]
    public void TryAddOrUpdate_WithNewKey_AddsValue()
    {
      var dict = new Dictionary<string, string>();

      var result = dict.TryAddOrUpdate("NewKey", "NewValue");

      Assert.That(result, Is.True);
      Assert.That(dict["NewKey"], Is.EqualTo("NewValue"));
    }

    [Test]
    public void TryAddOrUpdate_WithExistingKey_UpdatesValue()
    {
      var dict = new Dictionary<string, string>
      {
        ["ExistingKey"] = "OldValue"
      };

      var result = dict.TryAddOrUpdate("ExistingKey", "UpdatedValue");

      Assert.That(result, Is.True);
      Assert.That(dict["ExistingKey"], Is.EqualTo("UpdatedValue"));
    }

    [Test]
    public void TryAddOrUpdate_WithNullDictionary_ReturnsFalse()
    {
      Dictionary<string, string>? dict = null;

      var result = dict!.TryAddOrUpdate("Key", "Value");

      Assert.That(result, Is.False);
    }

    [Test]
    public void TryAddOrUpdate_WithNullKey_ReturnsFalse()
    {
      var dict = new Dictionary<string, string>();

      var result = dict.TryAddOrUpdate(null!, "Value");

      Assert.That(result, Is.False);
    }

    // Tests for TryAdd
    [Test]
    public void TryAdd_WithNewKey_AddsValueAndReturnsTrue()
    {
      var dict = new Dictionary<string, string>();

      var result = dict.TryAdd("NewKey", "NewValue");

      Assert.That(result, Is.True);
      Assert.That(dict["NewKey"], Is.EqualTo("NewValue"));
    }

    [Test]
    public void TryAdd_WithExistingKey_ReturnsFalse()
    {
      var dict = new Dictionary<string, string>
      {
        ["ExistingKey"] = "OldValue"
      };

      var result = dict.TryAdd("ExistingKey", "NewValue");

      Assert.That(result, Is.False);
      Assert.That(dict["ExistingKey"], Is.EqualTo("OldValue"));
    }

    [Test]
    public void TryAdd_WithNullDictionary_ReturnsFalse()
    {
      IDictionary<string, string>? dict = null;

      var result = DictionaryHelper.TryAdd(dict!, "Key", "Value");

      Assert.That(result, Is.False);
    }

    [Test]
    public void TryAdd_WithNullKey_ReturnsFalse()
    {
      var dict = new Dictionary<string, string>();

      var result = DictionaryHelper.TryAdd<string, string>(dict, null!, "Value");

      Assert.That(result, Is.False);
    }

    // Tests for TryRemove
    [Test]
    public void TryRemove_WithExistingKey_RemovesAndReturnsTrue()
    {
      var dict = new Dictionary<string, string>
      {
        ["KeyToRemove"] = "ValueToRemove"
      };

      var result = dict.TryRemove("KeyToRemove", out var removedValue);

      Assert.That(result, Is.True);
      Assert.That(removedValue, Is.EqualTo("ValueToRemove"));
      Assert.That(dict.ContainsKey("KeyToRemove"), Is.False);
    }

    [Test]
    public void TryRemove_WithNonExistingKey_ReturnsFalse()
    {
      var dict = new Dictionary<string, string>
      {
        ["Key1"] = "Value1"
      };

      var result = dict.TryRemove("NonExistentKey", out var removedValue);

      Assert.That(result, Is.False);
      Assert.That(removedValue, Is.Null);
    }

    [Test]
    public void TryRemove_WithNullDictionary_ReturnsFalse()
    {
      Dictionary<string, string>? dict = null;

      var result = dict!.TryRemove("Key", out var removedValue);

      Assert.That(result, Is.False);
      Assert.That(removedValue, Is.Null);
    }

    [Test]
    public void TryRemove_WithNullKey_ReturnsFalse()
    {
      var dict = new Dictionary<string, string>
      {
        ["Key1"] = "Value1"
      };

      var result = dict.TryRemove(null!, out var removedValue);

      Assert.That(result, Is.False);
      Assert.That(removedValue, Is.Null);
    }
  }
}
