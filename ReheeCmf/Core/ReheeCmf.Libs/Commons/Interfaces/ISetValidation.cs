using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public interface ISetValidation : IIsvalidate
  {
    void SetValidation(params ValidationResult[] validations);
  }
}
