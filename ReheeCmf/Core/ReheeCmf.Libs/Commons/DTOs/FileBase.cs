using ReheeCmf.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  [DataContract]
  public abstract class FileBase : IFileInfo
  {
    [DataMember]
    public string? FileUUID { get; set; }
    [DataMember]
    public string? Path { get; set; }
    [DataMember]
    public string? ContentType { get; set; }
    [DataMember]
    public long Length { get; set; }
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public string? FileName { get; set; }

    public Stream? Content { get; set; }
    [DataMember]
    public string? FullPath { get; set; }
    [DataMember]
    public string? EntityType { get; set; }
    [DataMember]
    public string? EntityKey { get; set; }
    [DataMember]
    public string? PropertyName { get; set; }
    [DataMember]
    public int? ValueIndex { get; set; }

    public bool Auth { get; set; }
  }

  public class FileServiceResponse : FileBase
  {

  }
  public class FileServiceRequest : FileBase
  {

  }
}
