using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmfBlazorSSR.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetUserLogins",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetUserClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                table: "AspNetRoleClaims",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_ApplicationUserId",
                table: "AspNetUserClaims",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_ApplicationUserId",
                table: "AspNetUserClaims",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_ApplicationUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "RoleBasedPermissions");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_ApplicationUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetRoleClaims");
        }
    }
}
