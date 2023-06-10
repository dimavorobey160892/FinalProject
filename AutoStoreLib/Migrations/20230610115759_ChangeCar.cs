using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoStoreLib.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineId",
                table: "Cars");

            migrationBuilder.AddColumn<double>(
                name: "EngineСapacityMax",
                table: "Cars",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EngineСapacityMin",
                table: "Cars",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineСapacityMax",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EngineСapacityMin",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "EngineId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
