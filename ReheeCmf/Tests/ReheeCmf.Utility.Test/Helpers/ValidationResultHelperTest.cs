using System.ComponentModel.DataAnnotations;
using ReheeCmf.Helpers;
using ReheeCmf.Responses;

namespace ReheeCmf.Utility.Test.Helpers
{
  public class ValidationResultHelperTest
  {
    [Test]
    public void New_WithMessageAndKeys_ReturnsValidationResult()
    {
      var result = ValidationResultHelper.New("Test message", "Key1", "Key2");

      Assert.That(result.ErrorMessage, Is.EqualTo("Test message"));
      Assert.That(result.MemberNames, Does.Contain("Key1"));
      Assert.That(result.MemberNames, Does.Contain("Key2"));
    }

    [Test]
    public void New_WithMessageOnly_ReturnsValidationResultWithNoKeys()
    {
      var result = ValidationResultHelper.New("Test message");

      Assert.That(result.ErrorMessage, Is.EqualTo("Test message"));
      Assert.That(result.MemberNames, Is.Empty);
    }

    [Test]
    public void ValidationrResponse_WithNull_ReturnsEmptyContentResponse()
    {
      object? obj = null;

      var result = obj!.ValidationrResponse();

      Assert.That(result, Is.Not.Null);
      Assert.That(result.Validation, Is.Null);
    }

    [Test]
    public void ValidationrResponse_WithValidObject_ReturnsContentResponseWithNoErrors()
    {
      var obj = new ValidTestObject { Name = "Test" };

      var result = obj.ValidationrResponse();

      Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void ValidationrResponse_WithInvalidObject_ReturnsContentResponseWithErrors()
    {
      var obj = new InvalidTestObject { RequiredField = null };

      var result = obj.ValidationrResponse();

      Assert.That(result, Is.Not.Null);
      Assert.That(result.Validation, Is.Not.Null);
      Assert.That(result.Validation!.Count(), Is.GreaterThan(0));
    }

    private class ValidTestObject
    {
      public string? Name { get; set; }
    }

    private class InvalidTestObject
    {
      [Required]
      public string? RequiredField { get; set; }
    }
  }
}
