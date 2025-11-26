using System.Net;

namespace ReheeCmf.Helpers
{
	public static class ContentResponseHelper
	{
		public static ContentResponse SetSuccess(this ContentResponse response, object? contentResponse)
		{
			response.SetContent(contentResponse);
			response.Success = true;
			response.Status = HttpStatusCode.OK;
			return response;
		}
		public static ContentResponse<T> SetSuccess<T>(this ContentResponse<T> response, T? contentResponse)
		{
			response.Content = contentResponse;
			response.Success = true;
			response.Status = HttpStatusCode.OK;
			return response;
		}
		public static void SetMultiResponse<T>(this ContentResponse<IEnumerable<T>> that, IEnumerable<ContentResponse<T>> sources)
		{
			that.Success = sources.All(b => b.Success);
			that.Content = sources.Where(b => b.Content != null).Select(b => b.Content!);
			if (!that.Success)
			{
				that.ErrorMessage = String.Join(",", sources.Select(b => b.ErrorMessage));
			}
		}

		public static T SetError<T>(this T that, Exception exception, HttpStatusCode status = HttpStatusCode.InternalServerError) where T : ContentResponse
		{
			if (exception is StatusException statusEx)
			{
				that.ExceptionValidation = statusEx.ValidationError?.Select(b => b);
			}
			return that.SetError(status, exception.Message);
		}
		public static ContentResponse SetError(this ContentResponse that, ContentResponse response)
		{
			that.ErrorMessage = response.ErrorMessage;
			that.ErrorCode = response.ErrorCode;
			that.Status = response.Status;
			if (response.Validation != null)
			{
				that.SetValidation(response.Validation.ToArray());
			}
			return that;
		}
		public static T SetNotFound<T>(this T that, string? message = null) where T : ContentResponse
		{
			return that.SetError(HttpStatusCode.NotFound, message);
		}
		public static T SeNull<T>(this T that) where T : ContentResponse
		{
			return that.SetError(HttpStatusCode.BadRequest, "null instance");
		}
		public static T SetError<T>(this T that, ContentResponse error) where T : ContentResponse
		{
			return that.SetError(error.Status, error.ErrorMessage, error.ErrorCode);
		}

		public static T SetError<T>(this T error, HttpStatusCode status = HttpStatusCode.BadRequest, string? errorMessage = null, string? errorCode = null) where T : ContentResponse
		{
			error.Status = status;
			error.ErrorCode = errorCode;
			error.ErrorMessage = errorMessage;
			error.Success = false;
			return error;
		}

		public static ContentResponse<T> SetSuccess<T>(this ContentResponse<T> that, T input, HttpStatusCode status = HttpStatusCode.OK)
		{
			that.Content = input;
			that.Status = status;
			that.Success = true;
			return that;
		}

	}
}
