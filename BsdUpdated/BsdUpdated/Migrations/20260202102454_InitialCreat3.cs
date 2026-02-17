using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BsdUpdated.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Gift_GiftId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Gift_Donor_DonorId",
                table: "Gift");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_GiftId",
                table: "Basket",
                column: "GiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Gift_GiftId",
                table: "Basket",
                column: "GiftId",
                principalTable: "Gift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Gift_GiftId",
                table: "Card",
                column: "GiftId",
                principalTable: "Gift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_Donor_DonorId",
                table: "Gift",
                column: "DonorId",
                principalTable: "Donor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Gift_GiftId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_Gift_GiftId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Gift_Donor_DonorId",
                table: "Gift");

            migrationBuilder.DropIndex(
                name: "IX_Basket_GiftId",
                table: "Basket");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Gift_GiftId",
                table: "Card",
                column: "GiftId",
                principalTable: "Gift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_Donor_DonorId",
                table: "Gift",
                column: "DonorId",
                principalTable: "Donor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
