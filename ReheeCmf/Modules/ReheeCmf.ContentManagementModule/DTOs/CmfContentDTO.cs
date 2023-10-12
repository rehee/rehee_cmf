namespace ReheeCmf.ContentManagementModule.DTOs
{
  public class CmfContentDTO
  {
    public string EntityType => nameof(CmfContentDTO);
    public Guid Id { get; set; }
    public int IntValue { get; set; }

    public IQueryable<Dictionary<string, object>> Get(IContext context, bool ignoreTracking)
    {
      return

      from entityMeta in context.Query<CmsEntityMetadata>(ignoreTracking).Where(b => String.Equals(EntityType, b.EntityName))

      select new Dictionary<string, object>()
      {
        {
          "a",1
        }
      };
    }
  }
}

