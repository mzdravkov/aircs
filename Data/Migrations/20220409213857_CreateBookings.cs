using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace airbnb.Data.Migrations
{
    public partial class CreateBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HomeId = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    CanceledById = table.Column<string>(type: "TEXT", nullable: true),
                    CheckIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuestsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestNote = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_CanceledById",
                        column: x => x.CanceledById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Home_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Home",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CanceledById",
                table: "Bookings",
                column: "CanceledById");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuestId",
                table: "Bookings",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HomeId",
                table: "Bookings",
                column: "HomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
