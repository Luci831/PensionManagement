using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserDetailsMicroservice.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatId = table.Column<int>(type: "int", nullable: false),
                    SqId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "PensionerDetails",
                columns: table => new
                {
                    Prid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AadharNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Allowances = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ptid = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    PensionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PensionerDetails", x => x.Prid);
                    table.ForeignKey(
                        name: "FK_PensionerDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PensionerDetails_UserId",
                table: "PensionerDetails",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PensionerDetails");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
