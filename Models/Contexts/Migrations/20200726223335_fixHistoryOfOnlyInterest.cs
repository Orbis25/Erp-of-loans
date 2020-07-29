using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP_SPARTAN.Data.Migrations
{
    public partial class fixHistoryOfOnlyInterest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryOnlyInterests_Loans_LoanId",
                table: "HistoryOnlyInterests");

            migrationBuilder.DropIndex(
                name: "IX_HistoryOnlyInterests_LoanId",
                table: "HistoryOnlyInterests");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "HistoryOnlyInterests");

            migrationBuilder.AddColumn<Guid>(
                name: "DebId",
                table: "HistoryOnlyInterests",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOnlyInterests_DebId",
                table: "HistoryOnlyInterests",
                column: "DebId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryOnlyInterests_Debs_DebId",
                table: "HistoryOnlyInterests",
                column: "DebId",
                principalTable: "Debs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryOnlyInterests_Debs_DebId",
                table: "HistoryOnlyInterests");

            migrationBuilder.DropIndex(
                name: "IX_HistoryOnlyInterests_DebId",
                table: "HistoryOnlyInterests");

            migrationBuilder.DropColumn(
                name: "DebId",
                table: "HistoryOnlyInterests");

            migrationBuilder.AddColumn<Guid>(
                name: "LoanId",
                table: "HistoryOnlyInterests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOnlyInterests_LoanId",
                table: "HistoryOnlyInterests",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryOnlyInterests_Loans_LoanId",
                table: "HistoryOnlyInterests",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
