namespace airbnb.Models;

public class Picture
{
    
    public int Id { get; set; }
    public virtual Home Home { get; set; }
    public string Filepath { get; set; }
}