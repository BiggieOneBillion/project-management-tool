using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class MakeTeamLeadIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TeamLeadId",
                table: "Projects",
                type: "character varying(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TeamLeadId",
                table: "Projects",
                type: "character varying(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldNullable: true);
        }
    }
}
