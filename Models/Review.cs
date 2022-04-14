using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airbnb.Models;

public class Review
{
    [ForeignKey("Booking")]
    public int Id { get; set; }
    [StringLength(1000)]
    public string Text { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public virtual Booking Booking { get; set; }
    public virtual User Guest { get; set; }
}