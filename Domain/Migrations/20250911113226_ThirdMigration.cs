using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__address__cust_id__3A81B327",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK__delivery__agent___60A75C0F",
                table: "delivery");

            migrationBuilder.DropForeignKey(
                name: "FK__delivery__order___5FB337D6",
                table: "delivery");

            migrationBuilder.DropForeignKey(
                name: "FK__delivery___agent__5CD6CB2B",
                table: "delivery_agent");

            migrationBuilder.DropForeignKey(
                name: "FK__food_item__categ__4316F928",
                table: "food_items");

            migrationBuilder.DropForeignKey(
                name: "FK__food_item__resta__4222D4EF",
                table: "food_items");

            migrationBuilder.DropForeignKey(
                name: "FK__order_ite__item___49C3F6B7",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK__order_ite__order__48CFD27E",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK__restauran__resta__3D5E1FD2",
                table: "restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK__review__restaura__5812160E",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK__review__user_id__571DF1D5",
                table: "review");

            migrationBuilder.CreateTable(
                name: "FoodByKeywords",
                columns: table => new
                {
                    item_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OrderDetailsByUserId",
                columns: table => new
                {
                    item_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.AddForeignKey(
                name: "FK__address__cust_id__3A81B327",
                table: "address",
                column: "cust_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__delivery__agent___60A75C0F",
                table: "delivery",
                column: "agent_id",
                principalTable: "delivery_agent",
                principalColumn: "agent_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__delivery__order___5FB337D6",
                table: "delivery",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__delivery___agent__5CD6CB2B",
                table: "delivery_agent",
                column: "agent_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__food_item__categ__4316F928",
                table: "food_items",
                column: "category_id",
                principalTable: "category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__food_item__resta__4222D4EF",
                table: "food_items",
                column: "restaurant_id",
                principalTable: "restaurant",
                principalColumn: "restaurant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__order_ite__item___49C3F6B7",
                table: "order_items",
                column: "item_id",
                principalTable: "food_items",
                principalColumn: "item_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__order_ite__order__48CFD27E",
                table: "order_items",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__restauran__resta__3D5E1FD2",
                table: "restaurant",
                column: "restaurant_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__review__restaura__5812160E",
                table: "review",
                column: "restaurant_id",
                principalTable: "restaurant",
                principalColumn: "restaurant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__review__user_id__571DF1D5",
                table: "review",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__address__cust_id__3A81B327",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK__delivery__agent___60A75C0F",
                table: "delivery");

            migrationBuilder.DropForeignKey(
                name: "FK__delivery__order___5FB337D6",
                table: "delivery");

            migrationBuilder.DropForeignKey(
                name: "FK__delivery___agent__5CD6CB2B",
                table: "delivery_agent");

            migrationBuilder.DropForeignKey(
                name: "FK__food_item__categ__4316F928",
                table: "food_items");

            migrationBuilder.DropForeignKey(
                name: "FK__food_item__resta__4222D4EF",
                table: "food_items");

            migrationBuilder.DropForeignKey(
                name: "FK__order_ite__item___49C3F6B7",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK__order_ite__order__48CFD27E",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK__restauran__resta__3D5E1FD2",
                table: "restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK__review__restaura__5812160E",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK__review__user_id__571DF1D5",
                table: "review");

            migrationBuilder.DropTable(
                name: "FoodByKeywords");

            migrationBuilder.DropTable(
                name: "OrderDetailsByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK__address__cust_id__3A81B327",
                table: "address",
                column: "cust_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__delivery__agent___60A75C0F",
                table: "delivery",
                column: "agent_id",
                principalTable: "delivery_agent",
                principalColumn: "agent_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__delivery__order___5FB337D6",
                table: "delivery",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__delivery___agent__5CD6CB2B",
                table: "delivery_agent",
                column: "agent_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK__food_item__categ__4316F928",
                table: "food_items",
                column: "category_id",
                principalTable: "category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__food_item__resta__4222D4EF",
                table: "food_items",
                column: "restaurant_id",
                principalTable: "restaurant",
                principalColumn: "restaurant_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__order_ite__item___49C3F6B7",
                table: "order_items",
                column: "item_id",
                principalTable: "food_items",
                principalColumn: "item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__order_ite__order__48CFD27E",
                table: "order_items",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__restauran__resta__3D5E1FD2",
                table: "restaurant",
                column: "restaurant_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__review__restaura__5812160E",
                table: "review",
                column: "restaurant_id",
                principalTable: "restaurant",
                principalColumn: "restaurant_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__review__user_id__571DF1D5",
                table: "review",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
