namespace ReheeCmf
{
	public abstract class Profile : IWithName, IWIthKeyType
	{
		public string? Name { get; set; }
		public string? Description { get; set; }

		public abstract Type KeyType { get; }

		public abstract int KeyValue { get; }

		public abstract string? StringKeyValue { get; }

		public string? StringKeyValueOverride { get; set; }

		public string? EffectiveKey => KeyValue != 0 ? StringKeyValue : StringKeyValueOverride;

		public string? Name1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
	public abstract class Profile<T> : Profile where T : Enum
	{
		public override Type KeyType => typeof(T);

		public abstract T Key { get; }

		public override string? StringKeyValue => Key?.ToString();

		public override int KeyValue => Key != null ? Convert.ToInt32(Key) : 0;
	}
}
