namespace ReheeCmf.ContextModule.Contexts
{
  public class CmfDbContext : DbContext, IWithContext
  {
    protected readonly IServiceProvider sp;
    public IContext? Context { get; set; }
    public CmfDbContext(IServiceProvider sp)
    {
      this.sp = sp;
    }

    
  }
}
