namespace ReheeCmf
{
  public abstract class Profile : IWithName, IWithNameOverride, IWIthKeyType, IWithKey
  {
    public virtual string Name => this.GetType().Name;
    public virtual string Description => Name;
    public abstract Type KeyType { get; }
    public abstract int KeyValue { get; }
    public abstract string? StringKeyValue { get; }
    public string? StringKeyValueOverride { get; set; }
    public string? NameOverride { get; set; }
    public string? DescriptionOverride { get; set; }
  }
  public abstract class Profile<T> : Profile where T : Enum
  {
    public abstract T Key { get; }
    public override Type KeyType => typeof(T);
    public override string? StringKeyValue => Key?.ToString();
    public override int KeyValue => Key != null ? Convert.ToInt32(Key) : 0;
  }
}
