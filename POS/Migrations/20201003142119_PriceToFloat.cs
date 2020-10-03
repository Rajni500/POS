using Microsoft.EntityFrameworkCore.Migrations;

namespace POS.Migrations
{
    public partial class PriceToFloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "VAT",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "SubTotal",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "InvoiceTotal",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "DiscountPercent",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VAT",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "SubTotal",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceTotal",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "DiscountPercent",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
