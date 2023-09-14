﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.ContextModule;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.EntityModule;
using ReheeCmf.Libs.Test.ContextsTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.Modules.ContextModuleTests
{
  internal abstract class ContextModuleTestBase<T> : ContextsTest<T> where T : DbContext
  {
    public IServiceProvider ServiceProvider { get; set; }
    public CmfContextModule<T, ReheeCmfBaseUser> CmfContextModule { get; set; }
    public CmfEntityModule CmfEntityModule { get; set; }
    [SetUp]
    public override void Setup()
    {
      base.Setup();
      ServiceProvider = ConfigService();
      CmfContextModule = new CmfContextModule<T, ReheeCmfBaseUser>();
      CmfEntityModule = new CmfEntityModule();
    }


  }
}
