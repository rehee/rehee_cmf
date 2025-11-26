using System.ComponentModel.DataAnnotations;
using System.Net;
using ReheeCmf.Commons;
using ReheeCmf.Helpers;
using ReheeCmf.Responses;

namespace ReheeCmf.Utility.Test.Commons
{
  public class StatusExceptionTest
  {
    [Test]
    public void Constructor_WithMessageOnly_SetsBadRequestStatus()
    {
      var ex = new StatusException("Test message");

      Assert.That(ex.Message, Is.EqualTo("Test message"));
      Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public void Constructor_WithMessageAndStatusCode_SetsProvidedStatus()
    {
      var ex = new StatusException("Not found", HttpStatusCode.NotFound);

      Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void Constructor_WithInnerException_PreservesInnerException()
    {
      var inner = new Exception("Inner");
      var ex = new StatusException("Outer", HttpStatusCode.BadRequest, inner);

      Assert.That(ex.InnerException, Is.SameAs(inner));
    }

    [Test]
    public void Constructor_WithErrorCode_SetsErrorCode()
    {
      var ex = new StatusException("Test", "ERR001", HttpStatusCode.BadRequest);

      Assert.That(ex.ErrorCode, Is.EqualTo("ERR001"));
    }

    [Test]
    public void StatusCodeInt_ReturnsIntegerStatusCode()
    {
      var ex = new StatusException("Test", HttpStatusCode.NotFound);

      Assert.That(ex.StatusCodeInt, Is.EqualTo(404));
    }

    [Test]
    public void Throw_WithStatusCodeAndMessage_ThrowsStatusException()
    {
      Assert.Throws<StatusException>(() =>
        StatusException.Throw(HttpStatusCode.Unauthorized, "Unauthorized access", "AUTH_ERR"));
    }

    [Test]
    public void Throw_WithContentResponse_ThrowsStatusException()
    {
      var response = new ContentResponse<string>();
      response.SetError(HttpStatusCode.BadRequest, "Error message", "ERR001");

      var ex = Assert.Throws<StatusException>(() => StatusException.Throw(response));

      Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
      Assert.That(ex.Message, Is.EqualTo("Error message"));
      Assert.That(ex.ErrorCode, Is.EqualTo("ERR001"));
    }

    [Test]
    public void Throw_WithValidationResults_ThrowsStatusExceptionWithValidation()
    {
      var validationResult = new ValidationResult("Field is required", new[] { "FieldName" });

      var ex = Assert.Throws<StatusException>(() =>
        StatusException.Throw(validationResult));

      Assert.That(ex.ValidationError, Is.Not.Null);
      Assert.That(ex.ValidationError!.Count(), Is.EqualTo(1));
    }

    [Test]
    public void ThrowOtherError_ThrowsStatusExceptionWithOtherError()
    {
      var errorObject = new { code = "ERR", details = "some details" };

      var ex = Assert.Throws<StatusException>(() =>
        StatusException.ThrowOtherError(errorObject));

      Assert.That(ex.OtherError, Is.EqualTo(errorObject));
    }

    [Test]
    public void ValidationError_CanBeSetAndRetrieved()
    {
      var response = new ContentResponse<string>();
      response.SetValidation(new ValidationResult("Error 1"), new ValidationResult("Error 2"));
      response.SetError(HttpStatusCode.BadRequest, "Validation failed");

      var ex = Assert.Throws<StatusException>(() => StatusException.Throw(response));

      Assert.That(ex.ValidationError, Is.Not.Null);
      Assert.That(ex.ValidationError!.Count(), Is.EqualTo(2));
    }
  }
}
