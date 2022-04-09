using airbnb.Data;
using airbnb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace airbnb.Controllers;

public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IFlashMessage _flashMessage;

    public BookingsController(ApplicationDbContext context, IFlashMessage flashMessage)
    {
        _context = context;
        _flashMessage = flashMessage;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    // }
    
    // GET: Home/Create
    public async Task<IActionResult> Create(int homeId, string dateRange, int guests)
    {
        var home = await _context.Home.FirstOrDefaultAsync(m => m.Id == homeId);
        if (home == null)
        {
            return NotFound();
        }
            
        BookingCreateViewModel bookingViewModel = new BookingCreateViewModel
        {
            DateRange = dateRange,
            Guests = guests,
            Home = home
        };
        return View(bookingViewModel);
    }
    
    // POST: Booking/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "DateRange", "Guests", "GuestNote", "HomeId")]
        BookingCreateViewModel bookingViewModel)
    {
        Console.WriteLine("LLAMA 0");
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
    
        Console.WriteLine("LLAMA 1");
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        
        var home = await _context.Home.FirstOrDefaultAsync(m => m.Id == bookingViewModel.HomeId);
        Console.WriteLine("LLAMA 1.5");
        Console.WriteLine(bookingViewModel.HomeId);
        Console.WriteLine("LLAMA 1.6");
        if (home == null)
        {
            return NotFound();
        }
        
        Console.WriteLine("LLAMA 2");

        if (!ModelState.IsValid)
        {
            _flashMessage.Danger("We couldn't process your booking request!");
            return RedirectToAction("Details", "Homes", new { home.Id });
        }
        Console.WriteLine("LLAMA 3");
        string[] dates = bookingViewModel.DateRange.Split(" - ");
        DateTime checkIn = DateTime.Parse(dates[0]);
        DateTime checkOut = DateTime.Parse(dates[1]);
        Console.WriteLine("LLAMA 4");

        Booking booking = new Booking
        {
            Home = home,
            Guest = user,
            Status = "created",
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestsCount = bookingViewModel.Guests,
            GuestNote = bookingViewModel.GuestNote
        };
        _context.Add(booking);
        _context.SaveChanges();
        
        _flashMessage.Confirmation("Your booking request was sent to the host.");
        return RedirectToAction("Details", "Homes", new { home.Id });
    }
}