using System.Runtime.Serialization;

namespace ReheeCmf.Commons.DTOs
{
	[DataContract]
	public class LoginDTO
	{
		[DataMember]
		[Required]
		public string? Username { get; set; }
		[DataMember]
		[Required(AllowEmptyStrings = true)]
		public string? Password { get; set; }
		[DataMember]
		public bool KeepLogin { get; set; }
		[DataMember]
		public string? ReturnUrl { get; set; }
	}
}
