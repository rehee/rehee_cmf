using System;

namespace ReheeCmf.Commons
{
	public interface IUserAndRole
	{
		Type? UserType { get; set; }
		Type? RoleType { get; set; }
	}
}
