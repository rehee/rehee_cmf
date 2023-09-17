using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmfDemo.Migrations
{
    /// <inheritdoc />
    public partial class override_user_claim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReheeCmfBaseUserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_ReheeCmfBaseUserId",
                table: "AspNetUserClaims",
                column: "ReheeCmfBaseUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_ReheeCmfBaseUserId",
                table: "AspNetUserClaims",
                column: "ReheeCmfBaseUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_ReheeCmfBaseUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_ReheeCmfBaseUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "ReheeCmfBaseUserId",
                table: "AspNetUserClaims");
        }
    }
}
