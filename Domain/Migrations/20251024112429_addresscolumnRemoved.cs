using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addresscolumnRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_food_items_FoodItemItemId",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "IX_order_items_FoodItemItemId",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "FoodItemItemId",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodItemItemId",
                table: "order_items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_FoodItemItemId",
                table: "order_items",
                column: "FoodItemItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_food_items_FoodItemItemId",
                table: "order_items",
                column: "FoodItemItemId",
                principalTable: "food_items",
                principalColumn: "item_id");
        }
    }
}
