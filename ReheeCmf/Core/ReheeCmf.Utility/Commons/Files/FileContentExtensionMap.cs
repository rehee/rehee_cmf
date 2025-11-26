using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Files
{
  public class FileContentExtensionMap
  {
    public FileContentExtensionMap(string extension, string contentType)
    {
      Extension = extension;
      ContentType = contentType;
    }

    public string Extension { get; }
    public string ContentType { get; }
  }
}
