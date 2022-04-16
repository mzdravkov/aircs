
namespace airbnb.Models;

public class Chat
{
    public int Id { get; set; }
    public User User1 { get; set; }
    public User User2 { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}