using System.Net;
using ReheeCmf.Helpers;
using ReheeCmf.Responses;

namespace ReheeCmf.Utility.Test.Helpers
{
  public class ContentResponseHelperTest
  {
    [Test]
    public void SetSuccess_WithContent_SetsSuccessAndOkStatus()
    {
      var response = new ContentResponse<string>();

      response.SetSuccess("test content");

      Assert.That(response.Success, Is.True);
      Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
      Assert.That(response.Content, Is.EqualTo("test content"));
    }

    [Test]
    public void SetSuccess_WithNullContent_SetsSuccessAndOkStatus()
    {
      var response = new ContentResponse<string>();

      response.SetSuccess((string?)null);

      Assert.That(response.Success, Is.True);
      Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
      Assert.That(response.Content, Is.Null);
    }

    [Test]
    public void SetError_SetsErrorStatus()
    {
      var response = new ContentResponse<string>();

      response.SetError(HttpStatusCode.BadRequest, "Error message");

      Assert.That(response.Success, Is.False);
      Assert.That(response.Status, Is.EqualTo(HttpStatusCode.BadRequest));
      Assert.That(response.ErrorMessage, Is.EqualTo("Error message"));
    }

    [Test]
    public void SetNotFound_SetsNotFoundStatus()
    {
      var response = new ContentResponse<string>();

      response.SetNotFound("Not found");

      Assert.That(response.Success, Is.False);
      Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NotFound));
      Assert.That(response.ErrorMessage, Is.EqualTo("Not found"));
    }

    [Test]
    public void SeNull_SetsBadRequestWithNullMessage()
    {
      var response = new ContentResponse<string>();

      response.SeNull();

      Assert.That(response.Success, Is.False);
      Assert.That(response.Status, Is.EqualTo(HttpStatusCode.BadRequest));
      Assert.That(response.ErrorMessage, Is.EqualTo("null instance"));
    }

    [Test]
    public void SetError_FromAnotherResponse_CopiesErrorDetails()
    {
      var errorResponse = new ContentResponse<string>();
      errorResponse.SetError(HttpStatusCode.Unauthorized, "Auth failed", "AUTH_ERR");

      var newResponse = new ContentResponse<int>();
      newResponse.SetError(errorResponse);

      Assert.That(newResponse.Success, Is.False);
      Assert.That(newResponse.Status, Is.EqualTo(HttpStatusCode.Unauthorized));
      Assert.That(newResponse.ErrorMessage, Is.EqualTo("Auth failed"));
      Assert.That(newResponse.ErrorCode, Is.EqualTo("AUTH_ERR"));
    }

    [Test]
    public void SetMultiResponse_WithAllSuccess_SetsSuccessTrue()
    {
      var responses = new List<ContentResponse<string>>
      {
        new ContentResponse<string> { Success = true, Content = "a" },
        new ContentResponse<string> { Success = true, Content = "b" }
      };
      var multiResponse = new ContentResponse<IEnumerable<string>>();

      multiResponse.SetMultiResponse(responses);

      Assert.That(multiResponse.Success, Is.True);
      Assert.That(multiResponse.Content!.Count(), Is.EqualTo(2));
    }

    [Test]
    public void SetMultiResponse_WithOneFailed_SetsSuccessFalse()
    {
      var responses = new List<ContentResponse<string>>
      {
        new ContentResponse<string> { Success = true, Content = "a" },
        new ContentResponse<string> { Success = false, Content = null, ErrorMessage = "Error" }
      };
      var multiResponse = new ContentResponse<IEnumerable<string>>();

      multiResponse.SetMultiResponse(responses);

      Assert.That(multiResponse.Success, Is.False);
      Assert.That(multiResponse.ErrorMessage, Does.Contain("Error"));
    }
  }
}
