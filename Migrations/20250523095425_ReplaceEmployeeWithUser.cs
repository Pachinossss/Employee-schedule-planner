using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeSchedulePlanner.Migrations
{
    public partial class ReplaceEmployeeWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjects_Employees_EmployeeId",
                table: "EmployeeProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Shifts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                newName: "IX_Shifts_UserId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeProjects",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjects_Users_UserId",
                table: "EmployeeProjects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Users_UserId",
                table: "Shifts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjects_Users_UserId",
                table: "EmployeeProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Users_UserId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Shifts",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_UserId",
                table: "Shifts",
                newName: "IX_Shifts_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmployeeProjects",
                newName: "EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjects_Employees_EmployeeId",
                table: "EmployeeProjects",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}