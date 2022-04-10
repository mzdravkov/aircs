namespace airbnb.Models;

public class HomeAndAmenitiesViewModel
{
    public Home Home { get; set; }
    public ICollection<Amenity> Amenities { get; set; }
}