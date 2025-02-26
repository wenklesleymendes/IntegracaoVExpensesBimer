using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTituloAprovado",
                table: "TituloAprovadoDespesa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TituloAprovadoDespesa_IdTituloAprovado",
                table: "TituloAprovadoDespesa",
                column: "IdTituloAprovado");

            migrationBuilder.AddForeignKey(
                name: "FK_TituloAprovadoDespesa_TitulosAprovados_IdTituloAprovado",
                table: "TituloAprovadoDespesa",
                column: "IdTituloAprovado",
                principalTable: "TitulosAprovados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TituloAprovadoDespesa_TitulosAprovados_IdTituloAprovado",
                table: "TituloAprovadoDespesa");

            migrationBuilder.DropIndex(
                name: "IX_TituloAprovadoDespesa_IdTituloAprovado",
                table: "TituloAprovadoDespesa");

            migrationBuilder.DropColumn(
                name: "IdTituloAprovado",
                table: "TituloAprovadoDespesa");
        }
    }
}
