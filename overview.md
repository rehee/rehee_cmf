# ReheeCmf 项目概述 (Project Overview)

> **版本**: 10.0.0-rc-2-002  
> **框架**: .NET 10.0  
> **类型**: Content Management Framework (内容管理框架)

## 一、项目简介

ReheeCmf 是一个基于 .NET 的快速开发框架，专为外包项目设计。其主要目标是：
- 提供标准化的 CRUD 操作
- 模块化架构设计
- 多租户支持
- 统一的权限管理
- 丰富的辅助工具类

---

## 二、解决方案结构

```
ReheeCmf/
├── Core/                      # 核心库
│   ├── ReheeCmf.Utility      # 工具库（netstandard2.1，包含原Libs中的大部分功能）
│   ├── ReheeCmf.Libs         # 基础库（仅包含Global.cs，所有项目的基础依赖）
│   ├── ReheeCmf.Modules      # 模块化框架核心
│   ├── ReheeCmf.Servers      # Web服务器配置和启动
│   ├── ReheeCmf.Caches       # 缓存服务
│   └── ReheeCmf.ODatas       # OData 查询支持
├── Components/                # 组件库
│   └── ReheeCmf.ContextComponent  # 数据库上下文组件
├── Modules/                   # 功能模块
│   ├── ReheeCmf.ApiModule    # API 聚合模块
│   ├── ReheeCmf.AuthenticationModule  # 认证模块
│   ├── ReheeCmf.ContextModule   # 数据上下文模块
│   ├── ReheeCmf.EntityModule    # 实体CRUD模块
│   ├── ReheeCmf.FileModule      # 文件服务模块
│   └── ReheeCmf.UserManagementModule  # 用户管理模块
├── Webs/                      # 示例项目（Demo）
│   ├── CmfDemo               # 标准Demo
│   └── CmfBlazorSSR          # Blazor SSR Demo
└── Tests/                     # 测试项目
    └── ReheeCmf.Utility.Test # 工具库测试
```

---

## 三、核心库 (Core) 详解

### 3.0 ReheeCmf.Utility - 工具库

**功能定位**: 提供跨平台的通用工具类，支持netstandard2.1，可被.NET Core/.NET 5+/.NET Framework项目引用。此库现包含原ReheeCmf.Libs中的大部分功能代码。

**目录结构**:
```
ReheeCmf.Utility/
├── Attributes/        # 自定义特性（Attribute）
│   ├── AutowiredAttribute.cs       # 自动注入特性
│   ├── IgnoreMappingAttribute.cs   # 忽略映射特性
│   ├── IgnoreTenantAttribute.cs    # 忽略租户特性
│   ├── IgnoreUpdateAttribute.cs    # 忽略更新特性
│   ├── NoAutowiredAttribute.cs     # 不自动注入特性
│   └── QueryBeforeFilterAttribute.cs # 查询前过滤特性
├── Authenticates/     # 认证相关（从Libs迁移）
│   ├── AuthorizeOption.cs          # 授权选项
│   ├── CmfAuthorizeAttribute.cs    # CMF授权特性
│   ├── IAuthorize.cs               # 授权接口
│   ├── IDTOSignInManager.cs        # DTO登录管理接口
│   ├── IJWTService.cs              # JWT服务接口
│   ├── ReheeCmfIdentity.cs         # CMF身份认证
│   ├── TokenManagement.cs          # Token管理
│   └── UserManagementOption.cs     # 用户管理选项
├── Caches/            # 缓存相关（从Libs迁移）
│   ├── CacheBaseHelper.cs          # 缓存基础帮助类
│   ├── ICacheManager.cs            # 缓存管理接口
│   ├── ICmfCache.cs                # CMF缓存接口
│   ├── IKeyValueCaches.cs          # 键值缓存接口
│   └── QuerySecondCache.cs         # 查询二级缓存
├── ConstValues/       # 常量值（从Libs迁移）
│   └── ConstCrud.cs                # CRUD常量
├── Enums/             # 枚举定义（以Enum开头，默认值NotSpecified=0）
│   ├── EnumAuthorizeType.cs        # 授权类型枚举
│   ├── EnumBadgeType.cs            # 徽章类型枚举
│   ├── EnumEntityState.cs          # 实体状态枚举
│   ├── EnumFileService.cs          # 文件服务枚举
│   ├── EnumHttpMethod.cs           # HTTP方法枚举
│   ├── EnumIdType.cs               # ID类型枚举
│   ├── EnumInputType.cs            # 输入类型枚举
│   ├── EnumPropertyUpdateType.cs   # 属性更新类型枚举
│   ├── EnumSQLType.cs              # SQL类型枚举
│   └── EnumTokenType.cs            # Token类型枚举
├── Helpers/           # 辅助工具类（扩展方法，已合并Libs中的Helper）
│   ├── AttributeHelper.cs          # 特性相关扩展
│   ├── ClaimsHelper.cs             # Claims相关扩展（从Libs迁移）
│   ├── CommonHelper.cs             # 通用帮助方法
│   ├── ComponentAttributeHelper.cs # 组件特性帮助类（从Libs迁移）
│   ├── ComponentFactory.cs         # 组件工厂（从Libs迁移）
│   ├── DictionaryHelper.cs         # 字典相关扩展
│   ├── EnumIdTypeHelper.cs         # ID类型帮助方法
│   ├── EntityRelationHelper.cs     # 实体关系帮助类（从Libs迁移）
│   ├── ETagHelper.cs               # ETag帮助类（从Libs迁移）
│   ├── FileRequestHelper.cs        # 文件请求帮助类（从Libs迁移）
│   ├── IHttpDictionaryHelper.cs    # HTTP字典帮助接口（从Libs迁移）
│   ├── StandardInputHelper.cs      # 标准输入帮助类（从Libs迁移）
│   ├── StringHelper.cs             # 字符串相关扩展
│   ├── TenantHelper.cs             # 租户帮助类（从Libs迁移）
│   └── TypeHelper.cs               # 类型相关扩展
├── Modules/           # 模块相关（从Libs迁移）
│   ├── Components/
│   │   └── ModulePermissionComponent.cs
│   ├── Helpers/
│   │   └── ModulePermissionComponentHelper.cs
│   ├── Permissions/
│   │   ├── ConstCmfAuthenticationModule.cs
│   │   ├── ConstCmfUserManagementModule.cs
│   │   └── IModulePermission.cs
│   └── ConstModule.cs
├── MultiTenants/      # 多租户实现（从Libs迁移）
│   ├── IServiceWithTenant.cs
│   ├── ServiceWithTenant.cs
│   └── TenantConnection.cs
├── Requests/          # 请求相关（从Libs迁移）
│   ├── IGetHttpClient.cs
│   ├── IGetRequestTokenService.cs
│   ├── IRequestBase.cs
│   ├── IRequestClient.cs
│   ├── IServiceModuleMapping.cs
│   ├── IServiceModuleRequestFactory.cs
│   ├── RequestBase.cs
│   └── RequestClient.cs
└── ... (其他原有目录)
```

**核心类说明**:

| 类名 | 说明 |
|------|------|
| `TypeHelper` | 类型判断和处理的扩展方法（IsNullable, IsIEnumerable, IsImplement等） |
| `StringHelper` | 字符串处理扩展方法（SplitPascalCase, GetSystemType等） |
| `AttributeHelper` | 特性获取和判断的扩展方法 |
| `DictionaryHelper` | 字典操作的扩展方法（大小写不敏感键值查找等） |
| `EnumIdTypeHelper` | ID类型映射帮助类 |
| `RequestBase` | HTTP请求基类（从Libs迁移） |
| `ServiceWithTenant` | 支持多租户的服务基类（从Libs迁移） |

**项目配置**:
- TargetFramework: netstandard2.1
- RootNamespace: ReheeCmf
- 依赖包：
  - Microsoft.CSharp
  - Microsoft.Extensions.DependencyInjection
  - Microsoft.Extensions.Primitives
  - Newtonsoft.Json
  - System.ComponentModel.Annotations
  - System.Net.Http
  - System.Text.Json

---

### 3.1 ReheeCmf.Libs - 基础库

**功能定位**: 基础依赖库，大部分代码已迁移至ReheeCmf.Utility，现仅保留Global.cs用于全局设置。

**目录结构**:
```
ReheeCmf.Libs/
└── Global.cs          # 全局设置和配置
```

**依赖包**:
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Primitives
- Newtonsoft.Json
- ProjectReference: ReheeCmf.Utility

---

### 3.2 ReheeCmf.Modules - 模块化核心

**功能定位**: 提供模块化开发框架，支持模块依赖、生命周期管理、权限配置。

**核心类**:

| 类名 | 说明 |
|------|------|
| `ServiceModule` | 模块基类，定义模块生命周期方法 |
| `ModuleDependOn` | 模块依赖声明 |
| `ServiceConfigurationContext` | 服务配置上下文 |

**模块生命周期**:
```csharp
Constructor()                           // 构造函数
PreConfigureServicesAsync()             // 预配置服务
ConfigureServicesAsync()                // 配置服务（主要服务注册）
PostConfigureServicesAsync()            // 后配置服务
BeforePreApplicationInitializationAsync() // 应用初始化前
PreApplicationInitializationAsync()     // 预应用初始化
ApplicationInitializationAsync()        // 应用初始化
PostApplicationInitializationAsync()    // 后应用初始化
FinalRootConfigure()                    // 最终配置
```

**依赖包**:
- Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
- Swashbuckle.AspNetCore.SwaggerGen/SwaggerUI
- System.IdentityModel.Tokens.Jwt

---

### 3.3 ReheeCmf.Servers - 服务器启动

**功能定位**: 提供Web应用的启动配置和中间件。

**核心类**:

| 类名 | 说明 |
|------|------|
| `ReheeCmfServer` | 静态启动类，包含`WebStartUp<T>()`方法 |

**使用方式**:
```csharp
await ReheeCmfServer.WebStartUp<MyApiModule>(args);
```

**启动流程**:
1. 加载所有依赖模块
2. 配置服务（DI容器）
3. 配置中间件（认证、授权、多租户）
4. 配置路由和OData
5. 启动应用

**中间件**:
- `CmfMultiTenancyMiddleware` - 多租户识别
- `CmfMiddlewareAuthorization` - 授权中间件
- `CmfExceptionFilter` - 异常处理过滤器
- `CmfAuthorizationActionFilter` - 授权动作过滤器

**依赖包**:
- Dapr.AspNetCore
- Microsoft.EntityFrameworkCore
- Microsoft.AspNetCore.OData
- OData.Swagger

---

### 3.4 ReheeCmf.Caches - 缓存服务

**功能定位**: 提供内存缓存的封装，支持过期管理。

**核心类**:

| 类名 | 说明 |
|------|------|
| `CmfMemoryCashe<T>` | 泛型内存缓存，支持过期时间 |
| `ITypedMemoryCache<T>` | 类型化缓存接口 |
| `KeyValueCaches` | 键值缓存 |

---

### 3.5 ReheeCmf.ODatas - OData支持

**功能定位**: 提供OData查询端点的自动生成和配置。

**核心功能**:
- 自动生成EDM模型
- 支持$filter、$expand、$select、$orderby查询
- 自定义OData约定
- ETag支持

**核心方法**:
```csharp
builder.AddCmfOData()                    // 添加OData支持
builder.AddCmfOdataEndpoint()            // 添加OData端点
services.AddTypeQuery<T, K>()            // 添加类型查询
```

---

## 四、组件库 (Components) 详解

### 4.1 ReheeCmf.ContextComponent

**功能定位**: 数据库上下文的组件化支持。

**核心接口**:
- `IDbContextBuilder` - 数据库上下文构建器

**依赖包**:
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore

---

## 五、功能模块 (Modules) 详解

### 5.1 ReheeCmf.ApiModule - API聚合模块

**功能定位**: 聚合所有子模块，提供标准API启动配置。

**模块依赖**:
```csharp
CmfContextModule<TContext, TUser>
CmfEntityModule
CmfAuthenticationModule<TUser>
CmfUserManagementModule<TUser, TRole, TUserRole>
CmfFileModule
```

**使用方式**:
```csharp
public class MyApiModule : CmfApiModule<MyDbContext, MyUser>
{
    public override string ModuleTitle => "My API";
    public override string ModuleName => "MyApi";
}
```

---

### 5.2 ReheeCmf.AuthenticationModule - 认证模块

**功能定位**: 提供JWT认证、用户身份验证。

**功能特性**:
- JWT Token 生成和验证
- Identity 集成
- 角色权限管理

**依赖包**:
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.AspNetCore.Identity.EntityFrameworkCore

---

### 5.3 ReheeCmf.ContextModule - 数据上下文模块

**功能定位**: 提供Entity Framework Core的上下文配置和仓储实现。

**核心类**:

| 类名 | 说明 |
|------|------|
| `CmfDbContext` | 数据库上下文基类，支持多租户、Token用户 |
| `CmfRepositoryContext` | 仓储上下文实现，实现IContext接口 |
| `CmfIdentityContext` | Identity上下文 |

**数据库支持**:
- SQL Server
- PostgreSQL (Npgsql)
- SQLite
- In-Memory (测试用)

**依赖包**:
- Microsoft.EntityFrameworkCore.SqlServer
- Npgsql.EntityFrameworkCore.PostgreSQL
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.InMemory

---

### 5.4 ReheeCmf.EntityModule - 实体CRUD模块

**功能定位**: 提供实体的通用CRUD API端点。

**API端点**:
- `GET /data/{entityName}` - 查询实体（支持OData）
- `POST /data/{entityName}` - 创建实体
- `PUT /data/{entityName}/{id}` - 更新实体
- `DELETE /data/{entityName}/{id}` - 删除实体

**权限生成**:
自动为每个实体生成CRUD权限：
- `Get_{EntityName}`
- `Post_{EntityName}`
- `Put_{EntityName}`
- `Delete_{EntityName}`

---

### 5.5 ReheeCmf.FileModule - 文件服务模块

**功能定位**: 提供文件上传、下载、存储服务。

**存储支持**:
- Azure Blob Storage
- Dropbox

**依赖包**:
- Azure.Storage.Blobs
- Dropbox.Api

---

### 5.6 ReheeCmf.UserManagementModule - 用户管理模块

**功能定位**: 提供用户、角色、权限的管理API。

**API端点**:
- `/user/Read/{role?}` - 获取用户列表
- `/user/Roles` - 获取角色列表
- `/user/UserRoles` - 获取用户角色

**服务**:
- `IUserService` - 用户服务
- `IUserRoleService` - 用户角色服务

---

## 六、核心设计模式

### 6.1 实体基类模式

```csharp
// 支持多种ID类型
public class MyEntity : EntityBase<Guid>
{
    public string Name { get; set; }
}

public class MyEntity : EntityBase<int>
{
    public string Name { get; set; }
}
```

### 6.2 统一响应模式

```csharp
public ContentResponse<T>
{
    T? Content;                    // 响应内容
    bool Success;                  // 是否成功
    HttpStatusCode Status;         // HTTP状态码
    string? ErrorMessage;          // 错误信息
    IEnumerable<ValidationResult>? Validation;  // 验证结果
}
```

### 6.3 模块依赖模式

```csharp
public override IEnumerable<ModuleDependOn> Depends()
{
    return ModuleHelper.Depends(
        ModuleDependOn.New<CmfContextModule<TContext, TUser>>(),
        ModuleDependOn.New<CmfEntityModule>()
    );
}
```

### 6.4 多租户模式

```csharp
public interface ITenantContext
{
    Tenant? ThisTenant { get; }
    Guid? TenantID { get; set; }
    bool IgnoreTenant { get; }
    void SetTenant(Tenant tenant);
    void SetIgnoreTenant(bool ignore);
}
```

### 6.5 组件模式

```csharp
// 为实体添加变更处理器
[EntityChangeComponent<MyChangeHandler>]
public class MyEntity : EntityBase<Guid>
{
}
```

---

## 七、配置选项

### 7.1 CrudOption - CRUD配置

```json
{
  "CrudOption": {
    "SQLType": "SQLServer",
    "DefaultConnectionString": "...",
    "DefaultReadOnlyConnectionString": "..."
  }
}
```

### 7.2 ApiSetting - API配置

```json
{
  "ApiSetting": {
    "IsDapr": false,
    "RSAOption": {
      "RSAPrivateKey": "...",
      "RSAPublicKey": "..."
    }
  }
}
```

### 7.3 TenantSetting - 租户配置

```json
{
  "TenantSetting": {
    "CustomService": false,
    "TenantContext": false
  }
}
```

---

## 八、快速开始

### 8.1 创建新项目

1. 创建DbContext:
```csharp
public class MyDbContext : CmfIdentityContext<MyUser>
{
    public MyDbContext(IServiceProvider sp) : base(sp) { }
    
    public DbSet<MyEntity> MyEntities { get; set; }
}
```

2. 创建用户类:
```csharp
public class MyUser : IdentityUser, ICmfUser
{
    public Guid? TenantID { get; set; }
}
```

3. 创建API模块:
```csharp
public class MyApiModule : CmfApiModule<MyDbContext, MyUser>
{
    public override string ModuleTitle => "My API";
    public override string ModuleName => "MyApi";
}
```

4. 启动应用:
```csharp
await ReheeCmfServer.WebStartUp<MyApiModule>(args);
```

---

## 九、项目依赖关系图

```
ReheeCmf.Libs (基础)
    ↑
ReheeCmf.Caches ←───────────────────────────┐
    ↑                                       │
ReheeCmf.Modules ←──────────────────────────┼─────────────────┐
    ↑                                       │                 │
    ├── ReheeCmf.ODatas                     │                 │
    │       ↑                               │                 │
    └── ReheeCmf.Servers                    │                 │
            ↑                               │                 │
            │                               │                 │
    ReheeCmf.ContextComponent ──────────────┘                 │
            ↑                                                 │
    ReheeCmf.ContextModule ←──────────────────────────────────┤
            ↑                                                 │
    ReheeCmf.AuthenticationModule ←───────────────────────────┤
            ↑                                                 │
    ReheeCmf.EntityModule ←───────────────────────────────────┤
            ↑                                                 │
    ReheeCmf.FileModule ←─────────────────────────────────────┤
            ↑                                                 │
    ReheeCmf.UserManagementModule ←───────────────────────────┘
            ↑
    ReheeCmf.ApiModule (聚合模块)
            ↑
    用户项目 (Webs/CmfDemo, CmfBlazorSSR)
```

---

## 十、测试

测试项目位于 `Tests/` 目录下：

### ReheeCmf.Utility.Test
工具库测试，包含：
- TypeHelperTest - 类型处理扩展测试
- StringHelperTest - 字符串处理扩展测试
- AttributeHelperTest - 特性处理扩展测试
- DictionaryHelperTest - 字典处理扩展测试
- EnumIdTypeHelperTest - ID类型帮助方法测试

> **注意**: ReheeCmf.Libs.Test 已被删除，原有测试功能已整合至 ReheeCmf.Utility.Test。

---

## 十一、代码规范

根据 `rules.md`：
1. 缩进使用 **2个空格**，禁止Tab
2. C# 的 `if` 语句必须使用大括号，禁止单行写法
3. 修改代码时必须同步更新 `overview.md`
4. 新增功能需编写单元测试

---

## 十二、版本信息

- 当前版本: 10.0.0-rc-2-002
- .NET版本: .NET 10.0
- 版本配置文件: `ReheeCmf/version.props`

---

> **文档更新日期**: 2025-11-26  
> **维护说明**: 此文档需与代码保持同步更新
