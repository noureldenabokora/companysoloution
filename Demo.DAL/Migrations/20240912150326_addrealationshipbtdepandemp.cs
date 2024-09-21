using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addrealationshipbtdepandemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Employes",
                type: "int",
                nullable: true,
                defaultValue: 0);  

            migrationBuilder.CreateIndex(
                name: "IX_Employes_DepartmentId",
                table: "Employes",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Departments_DepartmentId",
                table: "Employes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Departments_DepartmentId",
                table: "Employes");

            migrationBuilder.DropIndex(
                name: "IX_Employes_DepartmentId",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Employes");
        }
    }
}
