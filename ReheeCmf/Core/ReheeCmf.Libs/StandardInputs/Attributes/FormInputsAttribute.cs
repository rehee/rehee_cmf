using ReheeCmf.StandardInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf
{
  public class FormInputsAttribute : Attribute, IStandardParameter
  {
    public EnumInputType InputType { get; set; } = EnumInputType.Text;
    public Type? RelatedEntity { get; set; }
    public int? Col { get; set; }
    public string? ColType { get; set; }
    public string? Min { get; set; }
    public string? Max { get; set; }
    public string? InputMask { get; set; }
    public bool ReadOnly { get; set; }
    public int DisplayOrder { get; set; }
  }
}
