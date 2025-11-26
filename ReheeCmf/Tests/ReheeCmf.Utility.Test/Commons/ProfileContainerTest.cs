namespace ReheeCmf.Utility.Test.Commons
{
  public class ProfileContainerTest
  {
    private TestProfileContainer _container = null!;

    [SetUp]
    public void Setup()
    {
      _container = new TestProfileContainer();
    }

    [Test]
    public void AddProfile_WithValidProfile_AddsSuccessfully()
    {
      var profile = new ConcreteTestProfile("TestValue");

      _container.AddProfile(profile);

      var result = _container.GetProfile("TestValue");
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.EffectiveKey(), Is.EqualTo("TestValue"));
    }

    [Test]
    public void AddProfile_WithNullProfile_DoesNotThrow()
    {
      // Should not throw when adding null
      _container.AddProfile(null!);
      Assert.Pass();
    }

    [Test]
    public void GetProfile_WithExistingKey_ReturnsProfile()
    {
      var profile = new ConcreteTestProfile("Key1");
      _container.AddProfile(profile);

      var result = _container.GetProfile("Key1");

      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.SameAs(profile));
    }

    [Test]
    public void GetProfile_WithNonExistingKey_ReturnsNull()
    {
      var result = _container.GetProfile("NonExistentKey");

      Assert.That(result, Is.Null);
    }

    [Test]
    public void GetProfile_WithEnumKey_ReturnsCorrectProfile()
    {
      var profile = new ConcreteTestProfile(TestEnumKey.Value1);
      _container.AddProfile(profile);

      var result = _container.GetProfile(TestEnumKey.Value1, null);

      Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetProfile_WithZeroEnumKeyAndOverride_ReturnsCorrectProfile()
    {
      var profile = new ConcreteTestProfile("CustomKey");
      _container.AddProfile(profile);

      var result = _container.GetProfile(TestEnumKey.NotSpecified, "CustomKey");

      Assert.That(result, Is.Not.Null);
      Assert.That(result!.EffectiveKey(), Is.EqualTo("CustomKey"));
    }

    [Test]
    public void RemoveProfile_WithExistingKey_RemovesAndReturnsTrue()
    {
      var profile = new ConcreteTestProfile("KeyToRemove");
      _container.AddProfile(profile);

      var result = _container.RemoveProfile("KeyToRemove", out var removedProfile);

      Assert.That(result, Is.True);
      Assert.That(removedProfile, Is.Not.Null);
      Assert.That(_container.GetProfile("KeyToRemove"), Is.Null);
    }

    [Test]
    public void RemoveProfile_WithNonExistingKey_ReturnsFalse()
    {
      var result = _container.RemoveProfile("NonExistent", out var removedProfile);

      Assert.That(result, Is.False);
      Assert.That(removedProfile, Is.Null);
    }

    [Test]
    public void RemoveProfile_WithNullKey_ReturnsFalse()
    {
      var result = _container.RemoveProfile(null!, out var removedProfile);

      Assert.That(result, Is.False);
      Assert.That(removedProfile, Is.Null);
    }

    [Test]
    public void RemoveProfile_WithEmptyKey_ReturnsFalse()
    {
      var result = _container.RemoveProfile(string.Empty, out var removedProfile);

      Assert.That(result, Is.False);
      Assert.That(removedProfile, Is.Null);
    }

    [Test]
    public void GetAllProfiles_WithMultipleProfiles_ReturnsAll()
    {
      var profile1 = new ConcreteTestProfile("Key1");
      var profile2 = new ConcreteTestProfile("Key2");
      _container.AddProfile(profile1);
      _container.AddProfile(profile2);

      var result = _container.GetAllProfiles().ToList();

      Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetAllProfiles_WhenEmpty_ReturnsEmpty()
    {
      var result = _container.GetAllProfiles();

      Assert.That(result, Is.Empty);
    }

    [Test]
    public void KeyType_ReturnsCorrectType()
    {
      Assert.That(_container.KeyType, Is.EqualTo(typeof(TestEnumKey)));
    }

    [Test]
    public void AddProfile_WithDuplicateKey_DoesNotOverwrite()
    {
      var profile1 = new ConcreteTestProfile("DuplicateKey");
      var profile2 = new ConcreteTestProfile("DuplicateKey");

      _container.AddProfile(profile1);
      _container.AddProfile(profile2);

      var result = _container.GetProfile("DuplicateKey");
      Assert.That(result, Is.SameAs(profile1));
    }
  }

  // Test types for ProfileContainer tests
  public enum TestEnumKey
  {
    NotSpecified = 0,
    Value1 = 1,
    Value2 = 2
  }

  public class ConcreteTestProfile : Profile<TestEnumKey>
  {
    private readonly TestEnumKey _key;
    private readonly string? _keyOverride;

    public ConcreteTestProfile(string keyOverride)
    {
      _key = TestEnumKey.NotSpecified;
      _keyOverride = keyOverride;
      StringKeyValueOverride = keyOverride;
    }

    public ConcreteTestProfile(TestEnumKey key)
    {
      _key = key;
    }

    public override TestEnumKey Key => _key;
  }

  public class TestProfileContainer : ProfileContainer<TestEnumKey, ConcreteTestProfile>
  {
  }
}
