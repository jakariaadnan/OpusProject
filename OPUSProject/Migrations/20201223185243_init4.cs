using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSProject.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfVehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Years",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufactureYear = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Years", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(nullable: true),
                    TypeOfVehicle = table.Column<int>(nullable: false),
                    ModelName = table.Column<string>(nullable: true),
                    ChessisNo = table.Column<string>(nullable: true),
                    EngineNo = table.Column<string>(nullable: true),
                    ManufactureYear = table.Column<string>(nullable: true),
                    CC = table.Column<string>(nullable: true),
                    Color = table.Column<int>(nullable: false),
                    LoadCapacity = table.Column<string>(nullable: true),
                    Accessories = table.Column<string>(nullable: true),
                    DeliveryDays = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    OfferDays = table.Column<int>(nullable: false),
                    TypeOfVehiclesId = table.Column<int>(nullable: true),
                    YearsId = table.Column<int>(nullable: true),
                    AccessoriesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarDetails_Accessories_AccessoriesId",
                        column: x => x.AccessoriesId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarDetails_TypeOfVehicles_TypeOfVehiclesId",
                        column: x => x.TypeOfVehiclesId,
                        principalTable: "TypeOfVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarDetails_Years_YearsId",
                        column: x => x.YearsId,
                        principalTable: "Years",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusId = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    ChallanNo = table.Column<int>(nullable: false),
                    MoneyReceiptNo = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    BankDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillInfos_CarDetails_CarId",
                        column: x => x.CarId,
                        principalTable: "CarDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillInfos_CustomerDetails_CusId",
                        column: x => x.CusId,
                        principalTable: "CustomerDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillInfos_CarId",
                table: "BillInfos",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_BillInfos_CusId",
                table: "BillInfos",
                column: "CusId");

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_AccessoriesId",
                table: "CarDetails",
                column: "AccessoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_TypeOfVehiclesId",
                table: "CarDetails",
                column: "TypeOfVehiclesId");

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_YearsId",
                table: "CarDetails",
                column: "YearsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillInfos");

            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "CarDetails");

            migrationBuilder.DropTable(
                name: "CustomerDetails");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "TypeOfVehicles");

            migrationBuilder.DropTable(
                name: "Years");
        }
    }
}
