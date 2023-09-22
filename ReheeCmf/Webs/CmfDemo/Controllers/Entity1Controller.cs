using CmfDemo.Data;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.EntityModule.Controllers.v1_0;

namespace CmfDemo.Controllers
{
  //[ApiController]
  //[Route("Api/Data/Entity1")]
  //public class Entity1Controller : DataApiControllerBase<Entity1, int>
  //{
  //  public Entity1Controller(IServiceProvider sp) : base(sp)
  //  {
  //  }
  //}

  [ApiController]
  [Route("Api/Dto/Entity1DTO")]
  public class Entity1DTOController : CmfDTOOOataController<Entity1DTO>
  {
    public Entity1DTOController(IServiceProvider sp) : base(sp)
    {
    }
  }
}
