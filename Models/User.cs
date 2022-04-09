using Microsoft.AspNetCore.Identity;

namespace airbnb.Models;

public class User : IdentityUser
{
    public virtual ICollection<Home> Homes { get; set; }
}