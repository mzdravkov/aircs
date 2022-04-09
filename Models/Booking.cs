using System.ComponentModel.DataAnnotations;

namespace airbnb.Models;

public class Booking
{
    public int Id { get; set; }
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    [Required] 
    public Home Home { get; set; }
    [Required] 
    public User Guest { get; set; }
    [StringLength(16)]
    [Required] 
    public string Status { get; set; }
    public User? CanceledBy { get; set; }
    [DataType(DataType.Date)]
    public DateTime CheckIn { get; set; }
    [DataType(DataType.Date)]
    public DateTime CheckOut { get; set; }
    [Range(1, Int32.MaxValue)]
    public int GuestsCount { get; set; }
    [StringLength(200)]
    public string? GuestNote { get; set; }
}