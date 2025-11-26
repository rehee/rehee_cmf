namespace ReheeCmf.Utility.Test.Commons
{
  public class ProfileTest
  {
    [Test]
    public void Profile_KeyType_ReturnsCorrectEnumType()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile.KeyType, Is.EqualTo(typeof(SimpleTestEnum)));
    }

    [Test]
    public void Profile_StringKeyValue_ReturnsKeyAsString()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile.StringKeyValue, Is.EqualTo("TestValue"));
    }

    [Test]
    public void Profile_KeyValue_ReturnsEnumIntValue()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile.KeyValue, Is.EqualTo(1));
    }

    [Test]
    public void Profile_EffectiveKey_WhenKeyValueNonZero_ReturnsStringKeyValue()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile.EffectiveKey(), Is.EqualTo("TestValue"));
    }

    [Test]
    public void Profile_EffectiveKey_WhenKeyValueZero_ReturnsStringKeyValueOverride()
    {
      var profile = new ZeroKeyTestProfile();
      profile.StringKeyValueOverride = "OverrideKey";

      Assert.That(profile.EffectiveKey(), Is.EqualTo("OverrideKey"));
    }

    [Test]
    public void Profile_EffectiveKey_WhenKeyValueZeroAndNoOverride_ReturnsNull()
    {
      var profile = new ZeroKeyTestProfile();

      Assert.That(profile.EffectiveKey(), Is.Null);
    }

    [Test]
    public void Profile_NameProperty_CanBeSetAndGet()
    {
      var profile = new SimpleTestProfile();


      Assert.That(profile.Name, Is.EqualTo(nameof(SimpleTestProfile)));
    }

    [Test]
    public void Profile_DescriptionProperty_CanBeSetAndGet()
    {
      var profile = new SimpleTestProfile();


      Assert.That(profile.Description, Is.EqualTo(nameof(SimpleTestProfile)));
    }

    [Test]
    public void Profile_NameOverride_CanBeSetAndGet()
    {
      var profile = new SimpleTestProfile();
      profile.NameOverride = "CustomName";

      Assert.That(profile.NameOverride, Is.EqualTo("CustomName"));
    }

    [Test]
    public void Profile_DescriptionOverride_CanBeSetAndGet()
    {
      var profile = new SimpleTestProfile();
      profile.DescriptionOverride = "CustomDescription";

      Assert.That(profile.DescriptionOverride, Is.EqualTo("CustomDescription"));
    }

    [Test]
    public void Profile_StringKeyValueOverride_CanBeSetAndGet()
    {
      var profile = new SimpleTestProfile();
      profile.StringKeyValueOverride = "CustomKeyValue";

      Assert.That(profile.StringKeyValueOverride, Is.EqualTo("CustomKeyValue"));
    }

    [Test]
    public void Profile_ImplementsIWithName()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile, Is.InstanceOf<IWithName>());
    }

    [Test]
    public void Profile_ImplementsIWithNameOverride()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile, Is.InstanceOf<IWithNameOverride>());
    }

    [Test]
    public void Profile_ImplementsIWIthKeyType()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile, Is.InstanceOf<IWIthKeyType>());
    }

    [Test]
    public void Profile_ImplementsIWithKey()
    {
      var profile = new SimpleTestProfile();

      Assert.That(profile, Is.InstanceOf<IWithKey>());
    }
  }

  // Test enum for Profile tests
  public enum SimpleTestEnum
  {
    NotSpecified = 0,
    TestValue = 1
  }

  // Test Profile implementation with non-zero key
  public class SimpleTestProfile : Profile<SimpleTestEnum>
  {
    public override SimpleTestEnum Key => SimpleTestEnum.TestValue;
  }

  // Test Profile implementation with zero key
  public class ZeroKeyTestProfile : Profile<SimpleTestEnum>
  {
    public override SimpleTestEnum Key => SimpleTestEnum.NotSpecified;
  }
}
