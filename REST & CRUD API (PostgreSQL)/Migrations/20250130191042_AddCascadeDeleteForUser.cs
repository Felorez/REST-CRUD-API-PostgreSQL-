using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserIINs_Users_UserId",
                table: "UserIINs");

            migrationBuilder.AddForeignKey(
                name: "FK_UserIINs_Users_UserId",
                table: "UserIINs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserIINs_Users_UserId",
                table: "UserIINs");

            migrationBuilder.AddForeignKey(
                name: "FK_UserIINs_Users_UserId",
                table: "UserIINs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
