using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFlowTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemModelRemoveEffectiveDateCategoryRemoveDelayDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DelayDays",
                table: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DelayDays",
                table: "Category",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
