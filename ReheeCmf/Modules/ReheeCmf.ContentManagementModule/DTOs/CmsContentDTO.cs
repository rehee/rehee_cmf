namespace ReheeCmf.ContentManagementModule.DTOs
{
  public class CmsContentDTO
  {
    public Guid? Id { get; set; }
    public Dictionary<string, object?>? Data { get; set; }
    public IEnumerable<CmsPropertyDTO>? Properties { get; set; }
  }

}

