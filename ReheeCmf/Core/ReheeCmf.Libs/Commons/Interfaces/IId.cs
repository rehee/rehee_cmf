﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public interface IId<T> where T : IComparable
  {
    T Id { get; set; }
  }
}
