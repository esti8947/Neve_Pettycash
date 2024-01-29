using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addDepartmentIdToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StoreName",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Expenses",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_DepartmentId",
                table: "Expenses",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Departments_DepartmentId",
                table: "Expenses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Departments_DepartmentId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_DepartmentId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Expenses");

            migrationBuilder.AlterColumn<string>(
                name: "StoreName",
                table: "Expenses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
