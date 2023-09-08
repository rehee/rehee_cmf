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
  public interface IDbContextBuilder : ICmfComponent
  {
    void OnConfiguring(DbContextOptionsBuilder optionsBuilder, IContext? context);
    void OnModelCreating(ModelBuilder builder, IContext? context);
  }
}
