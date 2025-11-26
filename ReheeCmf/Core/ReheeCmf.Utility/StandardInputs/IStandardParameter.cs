namespace ReheeCmf.StandardInputs
{
	public interface IStandardParameter
	{
		EnumInputType InputType { get; set; }
		Type? RelatedEntity { get; set; }
		int? Col { get; set; }
		string? ColType { get; set; }
		string? Min { get; set; }
		string? Max { get; set; }
		string? InputMask { get; set; }
		bool ReadOnly { get; set; }
		int DisplayOrder { get; set; }

	}
}
