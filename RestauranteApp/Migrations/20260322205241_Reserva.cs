using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestauranteApp.Migrations
{
    /// <inheritdoc />
    public partial class Reserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredienteProduto_Ingredientees_IngredientesId",
                table: "IngredienteProduto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredientees",
                table: "Ingredientees");

            migrationBuilder.RenameTable(
                name: "Ingredientees",
                newName: "Ingredientes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredientes",
                table: "Ingredientes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Mesas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Capacidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantidadePessoas = table.Column<int>(type: "int", nullable: false),
                    CodigoConfirmacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    MesaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Mesas_MesaId",
                        column: x => x.MesaId,
                        principalTable: "Mesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClienteId",
                table: "Reservas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MesaId",
                table: "Reservas",
                column: "MesaId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredienteProduto_Ingredientes_IngredientesId",
                table: "IngredienteProduto",
                column: "IngredientesId",
                principalTable: "Ingredientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredienteProduto_Ingredientes_IngredientesId",
                table: "IngredienteProduto");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredientes",
                table: "Ingredientes");

            migrationBuilder.RenameTable(
                name: "Ingredientes",
                newName: "Ingredientees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredientees",
                table: "Ingredientees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredienteProduto_Ingredientees_IngredientesId",
                table: "IngredienteProduto",
                column: "IngredientesId",
                principalTable: "Ingredientees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
