using System.Net;
using ReheeCmf.Commons;
using ReheeCmf.Helpers;

namespace ReheeCmf.Utility.Test.Helpers
{
  public class StatusExceptionHelperTest
  {
    // Tests for ThrowStatusException
    [Test]
    public void ThrowStatusException_WithNullException_ReturnsNewException()
    {
      Exception? ex = null;

      var result = ex!.ThrowStatusException();

      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.TypeOf<Exception>());
    }

    [Test]
    public void ThrowStatusException_WithStatusException_ReturnsSameException()
    {
      var statusEx = new StatusException("Test message", HttpStatusCode.BadRequest);

      var result = statusEx.ThrowStatusException();

      Assert.That(result, Is.SameAs(statusEx));
    }

    [Test]
    public void ThrowStatusException_WithRegularException_ReturnsStatusException()
    {
      var ex = new InvalidOperationException("Original message");

      var result = ex.ThrowStatusException();

      Assert.That(result, Is.TypeOf<StatusException>());
      Assert.That(result.Message, Is.EqualTo("Original message"));
    }

    [Test]
    public void ThrowStatusException_WithInnerException_PreservesInnerException()
    {
      var innerEx = new ArgumentException("Inner exception");
      var ex = new Exception("Outer message", innerEx);

      var result = ex.ThrowStatusException() as StatusException;

      Assert.That(result, Is.Not.Null);
      Assert.That(result!.InnerException, Is.SameAs(innerEx));
    }

    // Tests for Throw extension method
    [Test]
    public void Throw_WithNullException_ReturnsException()
    {
      Exception? ex = null;

      var result = ex.Throw();

      Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Throw_WithStatusException_ReturnsSameException()
    {
      var statusEx = new StatusException("Test", HttpStatusCode.NotFound);

      var result = statusEx.Throw();

      Assert.That(result, Is.SameAs(statusEx));
    }

    // Tests for GetStatusException
    [Test]
    public void GetStatusException_WithNullException_ReturnsNull()
    {
      Exception? ex = null;

      var result = StatusExceptionHelper.GetStatusException(ex!);

      Assert.That(result, Is.Null);
    }

    [Test]
    public void GetStatusException_WithStatusException_ReturnsSameException()
    {
      var statusEx = new StatusException("Test message", HttpStatusCode.InternalServerError);

      var result = statusEx.GetStatusException();

      Assert.That(result, Is.SameAs(statusEx));
    }

    [Test]
    public void GetStatusException_WithRegularException_ReturnsNewStatusException()
    {
      var ex = new Exception("Test message");

      var result = ex.GetStatusException();

      Assert.That(result, Is.Not.Null);
      Assert.That(result.Message, Is.EqualTo("Test message"));
    }

    [Test]
    public void GetStatusException_WithNestedStatusException_ReturnsInnerStatusException()
    {
      var innerStatusEx = new StatusException("Inner status", HttpStatusCode.Unauthorized);
      var outerEx = new Exception("Outer", innerStatusEx);

      var result = outerEx.GetStatusException();

      Assert.That(result, Is.SameAs(innerStatusEx));
    }

    [Test]
    public void GetStatusException_WithDeeplyNestedStatusException_ReturnsInnerStatusException()
    {
      var innerStatusEx = new StatusException("Deep status", HttpStatusCode.Forbidden);
      var middleEx = new Exception("Middle", innerStatusEx);
      var outerEx = new Exception("Outer", middleEx);

      var result = outerEx.GetStatusException();

      Assert.That(result, Is.SameAs(innerStatusEx));
    }
  }
}
