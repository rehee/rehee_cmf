using ReheeCmf;
using ReheeCmf.DIContainers;

namespace ReheeCmf.Utility.Test.DIContainers
{
  public class DIPoolTest
  {
    [SetUp]
    public void Setup()
    {
      // Reset DIPool before each test
      DIPool.Reset();
    }

    [TearDown]
    public void TearDown()
    {
      // Clean up after each test
      DIPool.Reset();
    }

    [Test]
    public void Initialize_CalledMultipleTimes_OnlyInitializesOnce()
    {
      Assert.That(DIPool.IsInitialized, Is.False);

      DIPool.Initialize();

      Assert.That(DIPool.IsInitialized, Is.True);

      // Calling Initialize again should not throw
      DIPool.Initialize();

      Assert.That(DIPool.IsInitialized, Is.True);
    }

    [Test]
    public void Reset_ClearsContainers()
    {
      DIPool.Initialize();
      Assert.That(DIPool.IsInitialized, Is.True);

      DIPool.Reset();

      Assert.That(DIPool.IsInitialized, Is.False);
    }

    [Test]
    public void GetProfile_WithNullKey_ReturnsEmpty()
    {
      DIPool.Initialize();

      var result = DIPool.GetProfile((string)null!);

      Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetProfile_WithEmptyKey_ReturnsEmpty()
    {
      DIPool.Initialize();

      var result = DIPool.GetProfile(string.Empty);

      Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetProfile_BeforeInitialization_ReturnsEmpty()
    {
      var result = DIPool.GetProfile("TestKey");

      Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllProfiles_BeforeInitialization_ReturnsEmpty()
    {
      var result = DIPool.GetAllProfiles<TestProfile>();

      Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllProfilesByKeyType_BeforeInitialization_ReturnsEmpty()
    {
      var result = DIPool.GetAllProfilesByKeyType<TestEnum>();

      Assert.That(result, Is.Empty);
    }

    [Test]
    public void RegisterComponent_WithNonICmfComponent_DoesNotThrow()
    {
      // This should not throw even though the attribute is not an ICmfComponent
      DIPool.RegisterComponent(new SerializableAttribute(), typeof(string));

      // No exception means success
      Assert.Pass();
    }

    [Test]
    public void TryGetController_WithNullName_ReturnsFalse()
    {
      var result = DIPool.TryGetController(null, out var controller);

      Assert.That(result, Is.False);
      Assert.That(controller, Is.Null);
    }

    [Test]
    public void TryGetController_WithEmptyName_ReturnsFalse()
    {
      var result = DIPool.TryGetController(string.Empty, out var controller);

      Assert.That(result, Is.False);
      Assert.That(controller, Is.Null);
    }
  }

  // Test enum for profile tests
  public enum TestEnum
  {
    NotSpecified = 0,
    Value1 = 1,
    Value2 = 2
  }

  // Test profile class for testing
  public class TestProfile : Profile<TestEnum>
  {
    public override TestEnum Key => TestEnum.Value1;
  }

  // Test profile container class for testing
  public class TestProfileContainer : ProfileContainer<TestEnum, TestProfile>
  {
  }
}
