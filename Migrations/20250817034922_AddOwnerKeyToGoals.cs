using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetasAhorro.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerKeyToGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerKey",
                table: "Goals",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GoalId1",
                table: "Deposits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goals_OwnerKey",
                table: "Goals",
                column: "OwnerKey");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_GoalId1",
                table: "Deposits",
                column: "GoalId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_Goals_GoalId1",
                table: "Deposits",
                column: "GoalId1",
                principalTable: "Goals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_Goals_GoalId1",
                table: "Deposits");

            migrationBuilder.DropIndex(
                name: "IX_Goals_OwnerKey",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_GoalId1",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "OwnerKey",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "GoalId1",
                table: "Deposits");
        }
    }
}
