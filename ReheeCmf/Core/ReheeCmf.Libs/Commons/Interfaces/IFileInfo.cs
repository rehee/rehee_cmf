using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Interfaces
{
  public interface IFileInfo
  {
    string? FileUUID { get; set; }
    string? Path { get; set; }
    string? ContentType { get; set; }
    long Length { get; set; }
    string? Name { get; set; }
    string? FileName { get; set; }
    string? FullPath { get; set; }
    string? EntityType { get; set; }
    string? EntityKey { get; set; }
    string? PropertyName { get; set; }
    int? ValueIndex { get; set; }
    Stream? Content { get; set; }

  }
}
