namespace airbnb.Models;

public class HomeAmenity
{
    public int Id { get; set; }
    public Home Home { get; set; }
    public Amenity Amenity { get; set; }
}