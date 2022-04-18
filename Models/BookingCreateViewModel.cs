using System.ComponentModel.DataAnnotations;

namespace airbnb.Models;

public class BookingCreateViewModel
{
    [Display(Name = "Period")]
    public string DateRange { get; set; }
    
    [Range(1, Int32.MaxValue)]
    public int Guests { get; set; }
    
    public Home? Home { get; set; }
    
    [Display(Name = "Note")]
    [StringLength(200)]
    public string? GuestNote { get; set; }
    
    public int? HomeId { get; set; }
}