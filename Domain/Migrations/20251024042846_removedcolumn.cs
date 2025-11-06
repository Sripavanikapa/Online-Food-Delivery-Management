using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class removedcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_order_items_order_id",
                table: "order_items");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "OrderDetailsByUserId",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CustAddressId",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrderDetailsByUserId",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "imageurl",
                table: "OrderDetailsByUserId",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "OrderDetailsByUserId",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FoodItemItemId",
                table: "order_items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageurl",
                table: "FoodByKeywords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "food_items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_items",
                table: "order_items",
                columns: new[] { "order_id", "item_id" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_food_items_FoodItemItemId",
                table: "order_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_items",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "IX_order_items_FoodItemItemId",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CustAddressId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrderDetailsByUserId");

            migrationBuilder.DropColumn(
                name: "imageurl",
                table: "OrderDetailsByUserId");

            migrationBuilder.DropColumn(
                name: "status",
                table: "OrderDetailsByUserId");

            migrationBuilder.DropColumn(
                name: "FoodItemItemId",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "imageurl",
                table: "FoodByKeywords");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "food_items");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "category");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "address");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderDetailsByUserId",
                newName: "price");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_order_id",
                table: "order_items",
                column: "order_id");
        }
    }
}
