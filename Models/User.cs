using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace airbnb.Models;

public class User : IdentityUser
{
    [StringLength(120, MinimumLength = 3)]
    public string Name { get; set; }
    public virtual ICollection<Home> Homes { get; set; }
    
    public virtual ICollection<Review> Reviews { get; set; }
}