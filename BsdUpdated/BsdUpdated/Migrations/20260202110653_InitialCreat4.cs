using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BsdUpdated.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Gift_Cost",
                table: "Gift");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Gift",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonorId1",
                table: "Gift",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gift_CategoryId1",
                table: "Gift",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Gift_DonorId1",
                table: "Gift",
                column: "DonorId1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Gift_Cost",
                table: "Gift",
                sql: "Cost >= 10 AND Cost <= 100");

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_Category_CategoryId1",
                table: "Gift",
                column: "CategoryId1",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_Donor_DonorId1",
                table: "Gift",
                column: "DonorId1",
                principalTable: "Donor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gift_Category_CategoryId1",
                table: "Gift");

            migrationBuilder.DropForeignKey(
                name: "FK_Gift_Donor_DonorId1",
                table: "Gift");

            migrationBuilder.DropIndex(
                name: "IX_Gift_CategoryId1",
                table: "Gift");

            migrationBuilder.DropIndex(
                name: "IX_Gift_DonorId1",
                table: "Gift");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Gift_Cost",
                table: "Gift");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Gift");

            migrationBuilder.DropColumn(
                name: "DonorId1",
                table: "Gift");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Gift_Cost",
                table: "Gift",
                sql: "Cost > 10 AND Cost < 100");
        }
    }
}
