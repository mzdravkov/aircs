using System.ComponentModel.DataAnnotations;

namespace airbnb.Models;

public class Home
{
    public int Id { get; set; }
    public User? Owner { get; set; }
    [StringLength(120, MinimumLength = 10)]
    [Required] 
    public string Name { get; set; }
    [StringLength(60)]
    [Required] 
    public string Country { get; set; }
    [StringLength(60)]
    [Required] 
    public string Area { get; set; }
    [StringLength(60)]
    [Required] 
    public string City { get; set; }
    [StringLength(100)]
    [Required] 
    public string Street { get; set; }
    [StringLength(60)]
    [Required] 
    public string Number { get; set; }
    [StringLength(60)]
    [Required] 
    public string Apartment { get; set; }
    [StringLength(2000, MinimumLength = 100)]
    [Required] 
    public string Description { get; set; }
    [StringLength(1000, MinimumLength = 10)]
    [Required] 
    [Display(Name = "Check-in instructions")] 
    public string CheckInInstructions { get; set; }
    [StringLength(60)]
    [Required] 
    public string Type { get; set; }
    [Range(0, Int32.MaxValue)]
    [Display(Name = "Guests limit")] 
    public int GuestLimit { get; set; }
    [Range(0, Int32.MaxValue)]
    public int Bedrooms { get; set; }
    [Range(0, Int32.MaxValue)]
    public int Beds { get; set; }
    [Range(0, Int32.MaxValue)]
    public int Baths { get; set; }
    public virtual ICollection<Picture>? Pictures { get; set; }
    public virtual ICollection<Booking>? Bookings { get; set; }
    public virtual ICollection<HomeAmenity>? HomeAmenities { get; set; }

    public double Rating()
    {
        return Bookings
            .Where(booking => booking.Review != null)
            .Select(booking => booking.Review.Rating)
            .DefaultIfEmpty(0)
            .Average();
    }
}