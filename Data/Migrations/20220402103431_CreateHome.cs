using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace airbnb.Data.Migrations
{
    public partial class CreateHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Home",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Area = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Number = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Apartment = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    CheckInInstructions = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    GuestLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    Bedrooms = table.Column<int>(type: "INTEGER", nullable: false),
                    Beds = table.Column<int>(type: "INTEGER", nullable: false),
                    Baths = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Home", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Home_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Home_OwnerId",
                table: "Home",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Home");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
