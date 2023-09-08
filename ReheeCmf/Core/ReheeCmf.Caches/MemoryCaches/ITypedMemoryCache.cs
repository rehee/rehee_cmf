using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches.MemoryCaches
{
  public interface ITypedMemoryCache<T> : IMemoryCache
  {

  }
}
