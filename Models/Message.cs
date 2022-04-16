namespace airbnb.Models;

public class Message
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public User Author { get; set; }
    public string Text { get; set; }
    public virtual Chat Chat { get; set; }
}