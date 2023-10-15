using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.CodeAnalyses
{
  public class CmfCodeAnalysisOption
  {
    [NotNull]
    public string? Template { get; set; }
  }
  public class CmfCodeAnalysisOption<T> : CmfCodeAnalysisOption
  {

  }
}
