using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmfDemo.Migrations
{
	/// <inheritdoc />
	public partial class update_entity_type : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{


			migrationBuilder.CreateTable(
					name: "EntityType1s",
					columns: table => new
					{
						Id = table.Column<int>(type: "int", nullable: false)
									.Annotation("SqlServer:Identity", "1, 1"),
						Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
						TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_EntityType1s", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "EntityType2s",
					columns: table => new
					{
						Id = table.Column<int>(type: "int", nullable: false)
									.Annotation("SqlServer:Identity", "1, 1"),
						Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
						EntityType1Id = table.Column<int>(type: "int", nullable: true),
						TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_EntityType2s", x => x.Id);
						table.ForeignKey(
											name: "FK_EntityType2s_EntityType1s_EntityType1Id",
											column: x => x.EntityType1Id,
											principalTable: "EntityType1s",
											principalColumn: "Id");
					});

			migrationBuilder.CreateIndex(
					name: "IX_EntityType2s_EntityType1Id",
					table: "EntityType2s",
					column: "EntityType1Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "EntityType2s");

			migrationBuilder.DropTable(
					name: "EntityType1s");


		}
	}
}
