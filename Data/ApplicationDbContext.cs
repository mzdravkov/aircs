using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using airbnb.Models;

namespace airbnb.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<airbnb.Models.Home> Home { get; set; }
    public DbSet<airbnb.Models.Picture> Picture { get; set; }
    public DbSet<airbnb.Models.Booking> Bookings { get; set; }
    public DbSet<airbnb.Models.Amenity> Amenities { get; set; }
}