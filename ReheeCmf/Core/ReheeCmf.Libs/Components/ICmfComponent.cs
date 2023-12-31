﻿using ReheeCmf.Utility.CmfRegisters;

namespace ReheeCmf.Components
{
  public interface ICmfComponent : IRegistrableAttribute
  {
    ICmfHandler? CreateHandler();
    ICmfHandler? SingletonHandler();
    THandler? SingletonHandler<THandler>() where THandler : ICmfHandler;
    THandler? CreateHandler<THandler>() where THandler : ICmfHandler;
    Type? HandlerType { get; set; }
    Type? EntityType { get; set; }
    Type? PropertyType { get; set; }
    int Index { get; }
    int SubIndex { get; }
    string? Group { get; }
    bool Unique { get; }
    bool SkipFollowing { get; }
  }
}
