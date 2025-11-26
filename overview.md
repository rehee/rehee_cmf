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
│   ├── ReheeCmf.Utility      # 工具库（netstandard2.1，通用工具类）
│   ├── ReheeCmf.Libs         # 基础库（所有项目的基础依赖）
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
    ├── ReheeCmf.Utility.Test # 工具库测试
    └── ReheeCmf.Libs.Test    # 基础库测试
```

---

## 三、核心库 (Core) 详解

### 3.0 ReheeCmf.Utility - 工具库

**功能定位**: 提供跨平台的通用工具类，支持netstandard2.1，可被.NET Core/.NET 5+/.NET Framework项目引用。

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
├── Commons/           # 公共接口（新增）
│   ├── IIsvalidate.cs              # 验证接口
│   └── ISetValidation.cs           # 设置验证接口
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
├── Helpers/           # 辅助工具类（扩展方法）
│   ├── AttributeHelper.cs          # 特性相关扩展
│   ├── CommonHelper.cs             # 通用帮助方法
│   ├── DictionaryHelper.cs         # 字典相关扩展
│   ├── EnumIdTypeHelper.cs         # ID类型帮助方法
│   ├── StringHelper.cs             # 字符串相关扩展
│   └── TypeHelper.cs               # 类型相关扩展
└── Responses/         # 响应模型（新增）
    ├── ContentResponse.cs          # 统一响应包装
    └── IContentResponse.cs         # 响应接口
```

**核心类说明**:

| 类名 | 说明 |
|------|------|
| `TypeHelper` | 类型判断和处理的扩展方法（IsNullable, IsIEnumerable, IsImplement等） |
| `StringHelper` | 字符串处理扩展方法（SplitPascalCase, GetSystemType等） |
| `AttributeHelper` | 特性获取和判断的扩展方法 |
| `DictionaryHelper` | 字典操作的扩展方法（大小写不敏感键值查找等） |
| `EnumIdTypeHelper` | ID类型映射帮助类 |
| `ContentResponse<T>` | 统一的API响应包装类，包含Success、Status、Content、Validation等 |

**项目配置**:
- TargetFramework: netstandard2.1
- RootNamespace: ReheeCmf
- 依赖包: System.ComponentModel.Annotations

---

### 3.1 ReheeCmf.Libs - 基础库

**功能定位**: 整个框架的基础，定义了所有核心接口、实体基类、帮助类等。依赖 ReheeCmf.Utility 项目。

**目录结构**:
```
ReheeCmf.Libs/
├── Entities/          # 实体相关
│   ├── EntityBase.cs         # 实体基类（泛型支持多种ID类型）
│   ├── IEntityBase.cs        # 实体接口定义
│   ├── ICmfUser.cs           # CMF用户接口
│   └── RoleBasedPermission.cs # 基于角色的权限实体
├── Contexts/          # 上下文接口
│   ├── IContext.cs           # 数据上下文接口
│   ├── IRepository.cs        # 仓储接口
│   └── ISaveChange.cs        # 保存变更接口
├── Services/          # 服务接口
│   ├── IUserService.cs       # 用户服务接口
│   ├── IFileService.cs       # 文件服务接口
│   └── IToken.cs             # Token服务接口
├── Attributes/        # 自定义特性（仅包含需要Libs依赖的特性）
│   ├── PermissionAttribute.cs    # 权限特性
│   ├── FindCheckAttribute.cs     # 查找检查特性
│   └── ReadCheckAttribute.cs     # 读取检查特性
├── Handlers/          # 处理器
│   ├── ChangeHandlers/       # 变更处理器
│   ├── ValidationHandlers/   # 验证处理器
│   └── EntityChangeHandlers/ # 实体变更处理器
├── Helpers/           # 辅助工具类（仅包含需要Libs依赖的扩展）
│   ├── StringHelper.cs           # 字符串处理（扩展）
│   ├── StringValueHelper.cs      # 字符串值处理
│   ├── TypeHelperExtensions.cs   # 类型处理扩展
│   ├── JsonHelper.cs             # JSON处理
│   └── ValidationResultHelper.cs # 验证结果处理
├── DTOProcessors/     # DTO处理器
│   └── CmfDTOBase.cs         # DTO基类
├── Responses/         # 响应模型（仅保留Error类）
│   └── Error.cs              # 错误信息
├── Requests/          # 请求相关
│   └── RequestBase.cs        # 请求基类
├── Commons/           # 公共类
│   ├── CrudOption.cs         # CRUD配置选项
│   ├── ApiSetting.cs         # API设置
│   └── StatusException.cs    # 状态异常
├── Components/        # 组件支持
│   └── ICmfComponent.cs      # 组件接口
├── StandardInputs/    # 标准输入
│   └── Properties/           # 属性定义
├── Tenants/           # 多租户支持
├── MultiTenants/      # 多租户实现
└── Reflects/          # 反射相关
    └── ReflectPools/         # 反射池
```

**核心类说明**:

| 类名 | 说明 |
|------|------|
| `EntityBase<T>` | 实体基类，支持泛型ID（Guid、int、long等），自动包含TenantID |
| `IContext` | 数据上下文核心接口，继承ISaveChange、IRepository、IWithTenant |
| `CmfDTOBase<T>` | DTO基类，实现IValidatableObject验证 |
| `Error` | 错误信息包装类 |

**依赖包**:
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Primitives
- Newtonsoft.Json
- ReheeCmf.Utility (项目引用)

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

### ReheeCmf.Libs.Test
基础库测试，包含：
- ContextsTest - 上下文测试
- HandlerTest - 处理器测试
- HelperTest - 辅助类测试
- UtilityTests - 工具类测试

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
