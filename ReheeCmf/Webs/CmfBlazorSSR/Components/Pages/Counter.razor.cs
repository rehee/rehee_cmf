using CmfBlazorSSR.Data;
using Google.Api;
using Microsoft.AspNetCore.Components;
using ReheeCmf.Contexts;

namespace CmfBlazorSSR.Components.Pages
{
  public partial class Counter
  {
    private int currentCount = 0;
    [Inject]
    public IContext? context { get; set; }
    private async Task IncrementCount()
    {
      await Task.CompletedTask;

      //list[2].TenantID = Guid.NewGuid();try{
      try
      {
        await context.AddAsync(new Entity1 { });
      }
      catch (Exception ex)
      {
      }
      try
      {
        await context.SaveChangesAsync(null);
        var list = context.Query<Entity1>(false).ToList();
        
      }
      catch
      {
        var a = 1;
      }

    }
  }
}