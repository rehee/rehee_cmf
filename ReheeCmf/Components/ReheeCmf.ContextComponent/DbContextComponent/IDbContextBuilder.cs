using Microsoft.EntityFrameworkCore;
using ReheeCmf.Components;
using ReheeCmf.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextComponent
{
  public interface IDbContextBuilder : ICmfHandler
  {
    void OnConfiguring(DbContextOptionsBuilder optionsBuilder, IServiceProvider sp, DbContext context);
    void OnModelCreating(ModelBuilder builder, IServiceProvider sp, DbContext context);
  }
}
