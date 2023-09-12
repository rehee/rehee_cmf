using CmfDemo;
using ReheeCmf.Handlers.EntityChangeHandlers;
using ReheeCmf.Utility.CmfRegisters;

CmfRegister.Init();
await ReheeCmfServer.WebStartUp<DemoModule>(args);