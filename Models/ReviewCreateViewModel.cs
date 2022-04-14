using System.ComponentModel.DataAnnotations;

namespace airbnb.Models;

public class ReviewCreateViewModel
{
    [StringLength(1000)]
    public string? Text { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public Booking? Booking { get; set; }
    public int? BookingId { get; set; }
}