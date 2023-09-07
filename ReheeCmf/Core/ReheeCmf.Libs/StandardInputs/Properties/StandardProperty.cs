using System.Runtime.Serialization;

namespace ReheeCmf.StandardInputs.Properties
{
  [DataContract]
  public class StandardProperty : IProperty, IStandardParameter
  {
    [DataMember]
    public string? Label { get; set; }
    [DataMember]
    public string? PropertyName { get; set; }
    [DataMember]
    public string? Value { get; set; }
    [DataMember]
    public KeyValueItemDTO[]? SelectItem { get; set; }
    [DataMember]
    public bool MultiSelect { get; set; }

    [DataMember]
    public EnumInputType InputType { get; set; }
    [DataMember]
    public Type? RelatedEntity { get; set; }
    [DataMember]
    public int? Col { get; set; }
    [DataMember]
    public string? ColType { get; set; }
    [DataMember]
    public string? Min { get; set; }
    [DataMember]
    public string? Max { get; set; }
    [DataMember]
    public string? InputMask { get; set; }
    [DataMember]
    public bool ReadOnly { get; set; }
    [DataMember]
    public int DisplayOrder { get; set; }
  }
}
