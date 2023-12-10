using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMediator.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticlePrices",
                columns: table => new
                {
                    ArticleCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PriceListID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlePrices", x => new { x.ArticleCode, x.PriceListID });
                    table.ForeignKey(
                        name: "FK_ArticlePrices_PriceLists_PriceListID",
                        column: x => x.PriceListID,
                        principalTable: "PriceLists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePrices_PriceListID",
                table: "ArticlePrices",
                column: "PriceListID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticlePrices");

            migrationBuilder.DropTable(
                name: "PriceLists");
        }
    }
}
