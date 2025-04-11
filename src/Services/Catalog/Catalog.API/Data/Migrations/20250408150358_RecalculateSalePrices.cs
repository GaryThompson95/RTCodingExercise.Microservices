namespace Catalog.API.Migrations
{
    public partial class RecalculateSalePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a new column to store the original SalePrice
            migrationBuilder.AddColumn<decimal>(
                name: "OriginalSalePrice",
                table: "Plates",
                type: "decimal(18,2)",
                nullable: true);

            // Store the current SalePrice into OriginalSalePrice
            migrationBuilder.Sql(
                "UPDATE Plates SET OriginalSalePrice = SalePrice"
            );

            // Recalculate the SalePrice with a 20% markup
            migrationBuilder.Sql(
                "UPDATE Plates SET SalePrice = PurchasePrice * 1.2 WHERE PurchasePrice IS NOT NULL"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore the original SalePrice from OriginalSalePrice
            migrationBuilder.Sql(
                "UPDATE Plates SET SalePrice = OriginalSalePrice"
            );

            // Remove the OriginalSalePrice column
            migrationBuilder.DropColumn(
                name: "OriginalSalePrice",
                table: "Plates"
            );
        }
    }
}
