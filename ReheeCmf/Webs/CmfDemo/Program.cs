using CmfDemo;
using ReheeCmf.Handlers.EntityChangeHandlers;

EntityChangeHandlerFactory.Init();
await ReheeCmfServer.WebStartUp<DemoModule>(args);