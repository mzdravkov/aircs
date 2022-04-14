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
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
    
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        
        var home = await _context.Home.FirstOrDefaultAsync(m => m.Id == bookingViewModel.HomeId);
        if (home == null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _flashMessage.Danger("We couldn't process your booking request!");
            return RedirectToAction("Details", "Homes", new { home.Id });
        }
        
        string[] dates = bookingViewModel.DateRange.Split(" - ");
        DateTime checkIn = DateTime.Parse(dates[0]);
        DateTime checkOut = DateTime.Parse(dates[1]);

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
        return RedirectToAction("Index", "Bookings");
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
    
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

        var bookings = _context.Bookings
            .Include(b => b.Review)
            .Include(b => b.Guest)
            .Include(b => b.Home)
            .ThenInclude(h => h.Pictures)
            .Include(b => b.Home)
            .ThenInclude(h => h.Owner)
            .Where(
                booking => booking.Guest == user || booking.Home.Owner == user
            );

        return View(bookings);
    }
    
    [HttpPost, ActionName("Cancel")]
    public async Task<IActionResult> Cancel(int bookingId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        
        var booking = await _context.Bookings
            .Include(b => b.Home)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
        
        if (booking == null)
        {
            return NotFound();
        }
        
        if (user != booking.Guest && user != booking.Home.Owner)
        {
            return Unauthorized("You don't have rights to cancel this booking!");
        }

        if (DateTime.Today >= booking.CheckOut)
        {
            return BadRequest("Cannot cancel completed bookings!");
        }

        booking.Status = "canceled";
        _context.Update(booking);
        _context.SaveChanges();
        
        _flashMessage.Confirmation("The booking has been canceled.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Accept")]
    public async Task<IActionResult> Accept(int bookingId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        
        var booking = await _context.Bookings
            .Include(b => b.Home)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
        
        if (booking == null)
        {
            return NotFound();
        }
        
        if (user != booking.Home.Owner)
        {
            return Unauthorized("You don't have rights to accept this booking!");
        }

        if (DateTime.Today >= booking.CheckIn)
        {
            return BadRequest("You have to accept the booking before it starts!");
        }

        booking.Status = "accepted";
        _context.Update(booking);
        _context.SaveChanges();
        
        _flashMessage.Confirmation("The booking has been accepted.");
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Home/CreateReview
    public async Task<IActionResult> CreateReview(int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Home)
            .FirstOrDefaultAsync(m => m.Id == bookingId);
        if (booking == null)
        {
            return NotFound();
        }
        
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

        if (booking.Guest != user)
        {
            _flashMessage.Danger("You can only change your own bookings!");
            return RedirectToAction(nameof(Index));
        }
        
        if (booking.Status != "accepted" || DateTime.Today < booking.CheckOut)
        {
            _flashMessage.Danger("You can only write a review on the day after check out or later.");
            return RedirectToAction(nameof(Index));
        }
        
        return View(new ReviewCreateViewModel { Booking = booking });
    }
    
    [HttpPost, ActionName("SaveReview")]
    public async Task<IActionResult> SaveReview(
        [Bind(
            "BookingId", "Rating", "Text")]
        ReviewCreateViewModel reviewCreateViewModel)
    {
        var booking = await _context.Bookings
            .Include(b => b.Home)
            .FirstOrDefaultAsync(m => m.Id == reviewCreateViewModel.BookingId);
        if (booking == null)
        {
            return NotFound();
        }
        
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

        if (booking.Guest != user)
        {
            return BadRequest("You can only leave reviews for your own bookings!");
        }
        
        if (booking.Status != "accepted" || DateTime.Today < booking.CheckOut)
        {
            return BadRequest("You can only write a review on the day after check out or later.");
        }

        if (booking.Review != null)
        {
            return BadRequest("You've already left a review for this booking.");
        }
        
        _context.Add(new Review
        {
            Guest = user,
            Booking = booking,
            Rating = reviewCreateViewModel.Rating,
            Text = reviewCreateViewModel.Text
        });
        _context.SaveChanges();
        
        _flashMessage.Confirmation("Thank you for your review!");
        return RedirectToAction(nameof(Index));
    }
}