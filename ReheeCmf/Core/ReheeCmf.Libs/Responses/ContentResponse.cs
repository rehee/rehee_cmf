using System.Net;

namespace ReheeCmf.Responses
{

	public abstract class ContentResponse : IContentResponse
	{
		public abstract object? ContentObject { get; set; }
		public bool Success { get; set; }
		public HttpStatusCode Status { get; set; }
		public int IntStatus => (int)Status;
		public string? ErrorMessage { get; set; }
		public string? ErrorCode { get; set; }
		public bool Cancel { get; set; }
		public object? ErrorObject { get; set; }

		public IEnumerable<ValidationResult>? Validation { get; set; }
		public ValidationResultDTO[]? ValidationResult { get; set; }
		public IEnumerable<ValidationResult>? ExceptionValidation { get; set; }


		public void SetValidation(params ValidationResult[] validations)
		{
			Validation = validations;
		}
		bool IsDispose { get; set; }

		public bool ValidationError => Validation?.Any() == true && ExceptionValidation?.Any() == true;

		public void Dispose()
		{
			if (IsDispose)
			{
				return;
			}
			IsDispose = true;
			ContentObject = null;
			Validation = null;
			ExceptionValidation = null;
			ErrorObject = null;
			GC.SuppressFinalize(this);
		}
	}
	public class ContentResponse<T> : ContentResponse, IContentResponse<T>
	{
		public override object? ContentObject
		{
			get
			{
				return Content;
			}
			set
			{
				if (value is T tv)
				{
					Content = tv;
				}
			}
		}
		public T? Content { get; set; }
	}
}
