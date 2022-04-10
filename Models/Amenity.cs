using System.ComponentModel.DataAnnotations;

namespace airbnb.Models;

public class Amenity
{
    public int Id { get; set; }
    [StringLength(48)]
    public string Name { get; set; }
    public virtual ICollection<HomeAmenity> HomeAmenities { get; set; }
}