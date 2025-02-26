using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20250226080815_v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "observation",
                table: "TituloAprovadoDespesa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "observation",
                table: "TituloAprovadoDespesa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
