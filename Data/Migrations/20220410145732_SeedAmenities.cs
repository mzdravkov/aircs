using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace airbnb.Data.Migrations
{
    public partial class SeedAmenities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Towels" },
                    { 2, "Shampoo" },
                    { 3, "Hot water" },
                    { 4, "Essentials" },
                    { 5, "Bed linens" },
                    { 6, "Hair dryer" },
                    { 7, "Kitchen" },
                    { 8, "Wifi" },
                    { 9, "Free parking on premises" },
                    { 10, "Free parking on the street" },
                    { 11, "Backyard" },
                    { 12, "Smoke alarm" },
                    { 13, "Carbon monoxide alarm" },
                    { 14, "Washer" },
                    { 15, "Hangers" },
                    { 16, "Iron" },
                    { 17, "Air conditioning" },
                    { 18, "Refrigerator" },
                    { 19, "Oven" },
                    { 20, "Stove" },
                    { 21, "Coffee maker" },
                    { 22, "BBQ grill" },
                    { 23, "Pets allowed" },
                    { 24, "Smoking allowed" },
                    { 25, "TV" },
                    { 26, "Private entrance" },
                    { 27, "Fire extinguisher" }
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
