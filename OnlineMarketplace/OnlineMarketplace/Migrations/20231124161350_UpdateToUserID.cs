using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_ApplicationUserId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_ApplicationUserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_AspNetUsers_ApplicationUserId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_ApplicationUserId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Review_ApplicationUserId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Product_ApplicationUserId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UserID",
                table: "Product",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_UserID",
                table: "Product",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserID",
                table: "Review",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_AspNetUsers_UserID",
                table: "Sale",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_UserID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserID",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_AspNetUsers_UserID",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Product_UserID",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Sale",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Review",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ApplicationUserId",
                table: "Sale",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ApplicationUserId",
                table: "Review",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ApplicationUserId",
                table: "Product",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_ApplicationUserId",
                table: "Product",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_ApplicationUserId",
                table: "Review",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_AspNetUsers_ApplicationUserId",
                table: "Sale",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
