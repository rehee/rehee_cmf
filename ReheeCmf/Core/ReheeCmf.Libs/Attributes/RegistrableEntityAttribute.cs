using ReheeCmf.Utility.CmfRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class RegistrableEntityAttribute : Attribute, IRegistrableAttribute
  {

  }
}
