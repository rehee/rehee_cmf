namespace ReheeCmf
{
	public interface IWithKey
	{
		int KeyValue { get; }
		string? StringKeyValue { get; }
		string? StringKeyValueOverride { get; set; }
	}
}
