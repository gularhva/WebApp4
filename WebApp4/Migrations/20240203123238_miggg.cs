using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp4.Migrations
{
    public partial class miggg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Schools_SchoolId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Students");

            migrationBuilder.CreateIndex(
                name: "IX_Students_School_Id",
                table: "Students",
                column: "School_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Schools_School_Id",
                table: "Students",
                column: "School_Id",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Schools_School_Id",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_School_Id",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Schools_SchoolId",
                table: "Students",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
