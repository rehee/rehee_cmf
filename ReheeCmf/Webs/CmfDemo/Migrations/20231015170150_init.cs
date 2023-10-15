using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmfDemo.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CmsEntityMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityNameNormalization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionRead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleRead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsSplitQuery = table.Column<bool>(type: "bit", nullable: true),
                    SelectedProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueryBeforeFilter = table.Column<bool>(type: "bit", nullable: true),
                    HideProperty = table.Column<bool>(type: "bit", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsEntityMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity1s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleBasedPermissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizationModuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizationRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleBasedPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantLevel = table.Column<int>(type: "int", nullable: true),
                    TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantSubDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomolizationTenantSubDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenceEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MainConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadConnectionStrings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileServiceType = table.Column<int>(type: "int", nullable: true),
                    FileCompression = table.Column<bool>(type: "bit", nullable: true),
                    FileCompressionFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileBaseFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileServerPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileAccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileMaxFileUploadSize = table.Column<long>(type: "bigint", nullable: true),
                    FileAllowedRoles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileAuthRequired = table.Column<bool>(type: "bit", nullable: true),
                    FileAllowedFileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReheeCmfBaseUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_ReheeCmfBaseUserId",
                        column: x => x.ReheeCmfBaseUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CmsEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CmsEntityMetadataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    PublishedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpPublishedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true, computedColumnSql: "CASE \r\n  WHEN [Status] = 2 THEN CAST(1 AS bit)\r\n  WHEN [Status] = 4 AND [PublishedDate] IS NOT NULL AND [PublishedDate] >= CONVERT(datetimeoffset, SYSDATETIMEOFFSET()) AT TIME ZONE 'UTC' THEN CAST(1 AS bit)\r\n  WHEN [Status] = 4 AND [UpPublishedDate] IS NOT NULL AND [UpPublishedDate] < CONVERT(datetimeoffset, SYSDATETIMEOFFSET()) AT TIME ZONE 'UTC' THEN CAST(0 AS bit)\r\n  ELSE CAST(0 AS bit)\r\nEND"),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CmsEntity_CmsEntityMetadata_CmsEntityMetadataId",
                        column: x => x.CmsEntityMetadataId,
                        principalTable: "CmsEntityMetadata",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CmsPropertyMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CmsEntityMetadataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyNameNormalization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    InputType = table.Column<int>(type: "int", nullable: false),
                    NotNull = table.Column<bool>(type: "bit", nullable: true),
                    Unique = table.Column<bool>(type: "bit", nullable: true),
                    PermissionCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionRead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleRead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsPropertyMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CmsPropertyMetadata_CmsEntityMetadata_CmsEntityMetadataId",
                        column: x => x.CmsEntityMetadataId,
                        principalTable: "CmsEntityMetadata",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CmsProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CmsEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CmsPropertyMetadataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<int>(type: "int", nullable: true),
                    ValueString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueBoolean = table.Column<bool>(type: "bit", nullable: true),
                    ValueInt16 = table.Column<short>(type: "smallint", nullable: true),
                    ValueInt32 = table.Column<int>(type: "int", nullable: true),
                    ValueInt64 = table.Column<long>(type: "bigint", nullable: true),
                    ValueSingle = table.Column<float>(type: "real", nullable: true),
                    ValueDouble = table.Column<double>(type: "float", nullable: true),
                    ValueDecimal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValueDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValueDateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CmsProperty_CmsEntity_CmsEntityId",
                        column: x => x.CmsEntityId,
                        principalTable: "CmsEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CmsProperty_CmsPropertyMetadata_CmsPropertyMetadataId",
                        column: x => x.CmsPropertyMetadataId,
                        principalTable: "CmsPropertyMetadata",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_ReheeCmfBaseUserId",
                table: "AspNetUserClaims",
                column: "ReheeCmfBaseUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntity_CmsEntityMetadataId",
                table: "CmsEntity",
                column: "CmsEntityMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsProperty_CmsEntityId",
                table: "CmsProperty",
                column: "CmsEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsProperty_CmsPropertyMetadataId",
                table: "CmsProperty",
                column: "CmsPropertyMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsPropertyMetadata_CmsEntityMetadataId",
                table: "CmsPropertyMetadata",
                column: "CmsEntityMetadataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CmsProperty");

            migrationBuilder.DropTable(
                name: "Entity1s");

            migrationBuilder.DropTable(
                name: "RoleBasedPermissions");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CmsEntity");

            migrationBuilder.DropTable(
                name: "CmsPropertyMetadata");

            migrationBuilder.DropTable(
                name: "CmsEntityMetadata");
        }
    }
}
