using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoStoreLib.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Orders",
                newName: "TypeOfFuelId");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "Orders",
                newName: "Coments");

            migrationBuilder.AddColumn<int>(
                name: "EngineId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GearboxId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxMileage",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPrice",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxYear",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinMileage",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinPrice",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinYear",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeOfBodyId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GearboxId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaxMileage",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaxYear",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MinMileage",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MinYear",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TypeOfBodyId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TypeOfFuelId",
                table: "Orders",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Coments",
                table: "Orders",
                newName: "Info");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
