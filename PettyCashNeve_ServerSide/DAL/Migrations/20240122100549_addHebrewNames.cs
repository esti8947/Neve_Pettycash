using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addHebrewNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpenseCategoryNameHeb",
                table: "ExpenseCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventCategoryNameHeb",
                table: "EventCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BuyerNameHeb",
                table: "Buyers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseCategoryNameHeb",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "EventCategoryNameHeb",
                table: "EventCategories");

            migrationBuilder.DropColumn(
                name: "BuyerNameHeb",
                table: "Buyers");
        }
    }
}
