namespace ReheeCmf.Authenticates
{
	public class AuthorizeOption : ServiceModuleOption
	{
		public EnumAuthorizeType AuthorizeType { get; set; }
		public bool CheckUserEveryRequest { get; set; }


		//const value
		public const string ApplicationRole = "Application";
	}
}
