using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoStoreLib.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineСapacityMax",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EngineСapacityMin",
                table: "Cars");

            migrationBuilder.AddColumn<double>(
                name: "MaxEngineСapacity",
                table: "Orders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinEngineСapacity",
                table: "Orders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EngineId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "EngineСapacity",
                table: "Cars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxEngineСapacity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MinEngineСapacity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EngineId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EngineСapacity",
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
    }
}
