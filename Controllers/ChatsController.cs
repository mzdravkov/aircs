using airbnb.Data;
using airbnb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace airbnb.Controllers;

public class ChatsController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly IFlashMessage _flashMessage;
    
    public ChatsController(ApplicationDbContext context, IFlashMessage flashMessage)
    {
        _context = context;
        _flashMessage = flashMessage;
    }

    public IActionResult Index()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            _flashMessage.Danger("You have to log in to access this page.");
            return RedirectToAction("Index", "Index");
        }
        
        var chats = from c in _context.Chats
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Where(c => c.User1.UserName == User.Identity.Name || c.User2.UserName == User.Identity.Name)
                .Include(c => c.Messages)
            select c;
        var orderedChats = chats
            .ToList()
            .OrderBy(chat => chat.Messages.MaxBy(m => m.Timestamp).Timestamp)
            .Reverse();
        return View(orderedChats);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            _flashMessage.Danger("You have to log in to access this page.");
            return RedirectToAction("Index", "Index");
        }
    
        var chat = await _context.Chats
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages)
            .ThenInclude(m => m.Author)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (chat == null)
        {
            return NotFound();
        }

        return View(chat);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(       
        [Bind("ChatId", "AuthorUserName", "Text")]
        MessageCreateViewModel messageCreateViewModel)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated || messageCreateViewModel.AuthorUserName != User.Identity.Name)
        {
            return Unauthorized();
        }
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        
        var chat = await _context.Chats
            .Include(c => c.User1)
            .Include(c => c.User2)
            .FirstOrDefaultAsync(m => m.Id == messageCreateViewModel.ChatId);
        if (chat == null)
        {
            return NotFound();
        }

        if (chat.User1 != user && chat.User2 != user)
        {
            return Unauthorized();
        }
        
        var message = new Message
        {
            Chat = chat,
            Author = user,
            Timestamp = DateTime.Now,
            Text = messageCreateViewModel.Text
        };
        _context.Add(message);
        
        _context.SaveChanges();

        return RedirectToAction("Details", new { chat.Id });
    }
}