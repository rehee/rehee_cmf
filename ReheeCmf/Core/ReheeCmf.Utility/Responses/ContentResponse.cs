using System.Net;

namespace ReheeCmf.Responses
{
	public abstract class ContentResponse : ISetValidation, IDisposable
	{
		public bool Success { get; set; }
		public HttpStatusCode Status { get; set; }
		public int IntStatus => (int)Status;

		public string? ErrorMessage { get; set; }
		public string? ErrorCode { get; set; }
		public bool Cancel { get; set; }
		public object? ErrorObject { get; set; }
		public IEnumerable<ValidationResult>? Validation { get; protected set; }
		public IEnumerable<ValidationResult>? ExceptionValidation { get; set; }
		public abstract object? ContentObject { get; }
		public abstract void SetContent(object? content);
		public virtual void Dispose()
		{
			Validation = null;
			ExceptionValidation = null;
			ErrorObject = null;
			GC.SuppressFinalize(this);
		}
		public bool ValidationError => Validation?.Any() == true;
		public void SetValidation(params ValidationResult[] validations)
		{
			Validation = Validation == null ? validations : Validation.Concat(validations);
		}

	}
	public class ContentResponse<T> : ContentResponse
	{
		public T? Content { get; set; }
		public override object? ContentObject => Content;
		public override void Dispose()
		{
			Content = default(T);
			base.Dispose();
		}

		public override void SetContent(object? content)
		{
			if (content is T c)
			{
				content = c;
			}
			else
			{
				content = default(T);
			}
		}
	}
}
