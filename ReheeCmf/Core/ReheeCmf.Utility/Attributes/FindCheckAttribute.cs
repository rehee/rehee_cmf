using ReheeCmf.Commons.DTOs;
using System;

namespace ReheeCmf.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class FindCheckAttribute : Attribute
	{
	}
	public delegate Func<T, bool> FindCheck<T>(TokenDTO user);
}
