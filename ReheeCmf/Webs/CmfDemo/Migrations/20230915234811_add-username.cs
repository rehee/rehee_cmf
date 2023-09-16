using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmfDemo.Migrations
{
    /// <inheritdoc />
    public partial class addusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizationModuleName2",
                table: "RoleBasedPermissions");

            migrationBuilder.DropColumn(
                name: "NormalizationRoleName2",
                table: "RoleBasedPermissions");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "NormalizationModuleName2",
                table: "RoleBasedPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizationRoleName2",
                table: "RoleBasedPermissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
