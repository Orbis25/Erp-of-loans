using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP_SPARTAN.Data.Migrations
{
    public partial class HistoryPaymentsLoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoryPaymentsLoan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Share = table.Column<int>(nullable: false),
                    outstanding = table.Column<int>(nullable: false),
                    ToPay = table.Column<double>(nullable: false),
                    EndBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtraMount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdLoan = table.Column<Guid>(nullable: false),
                    LoanId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryPaymentsLoan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryPaymentsLoan_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryPaymentsLoan_LoanId",
                table: "HistoryPaymentsLoan",
                column: "LoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryPaymentsLoan");
        }
    }
}
