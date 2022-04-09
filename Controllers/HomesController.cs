#nullable disable
using airbnb.Data;
using airbnb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace airbnb.Controllers
{
    public class HomesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFlashMessage _flashMessage;

        public HomesController(ApplicationDbContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        // GET: Home
        public async Task<IActionResult> Index(string location)
        {
            var homes = from h in _context.Home.Include(h => h.Pictures) select h;
            if (!String.IsNullOrEmpty(location))
            {
                homes = homes.Where(
                    home => home.Country!.ToLower().Contains(location.ToLower())
                            || home.Area!.ToLower().Contains(location.ToLower())
                            || home.City!.ToLower().Contains(location.ToLower())
                            || home.Street!.ToLower().Contains(location.ToLower()));
            }

            return View(await homes.ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Home
                .Include(h => h.Pictures)
                .Include(h => h.Bookings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            
            return View(home);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,Name,Country,Area,City,Street,Number,Apartment,Description,CheckInInstructions,Type,GuestLimit,Bedrooms,Beds,Baths")]
            Home home)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            home.Owner = user;
            
            if (ModelState.IsValid)
            {
                _context.Add(home);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(home);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Home.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }
            
            if (!CanEditHome(home))
            {
                _flashMessage.Danger("You cannot edit this home");
                return RedirectToAction("Index");
            }

            return View(home);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,Name,Country,Area,City,Street,Number,Apartment,Description,CheckInInstructions,Type,GuestLimit,Bedrooms,Beds,Baths")]
            Home home)
        {
            if (id != home.Id)
            {
                return NotFound();
            }
            
            if (!CanEditHome(home))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(home);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeExists(home.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(home);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Home
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            if (!CanEditHome(home))
            {
                _flashMessage.Danger("You cannot delete this home");
                return RedirectToAction("Index");
            }
            
            return View(home);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var home = await _context.Home.FindAsync(id);
            
            if (!CanEditHome(home))
            {
                return Unauthorized();
            }
            _context.Home.Remove(home);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeExists(int id)
        {
            return _context.Home.Any(e => e.Id == id);
        }

        private bool CanEditHome(Home home)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return false;
            }
            
            User user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null || user != home.Owner)
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> AddPictures(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Home
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            
            if (!CanEditHome(home))
            {
                _flashMessage.Danger("You cannot edit this home!");
                return RedirectToAction("Index");
            }

            return View(home);
        }
        
        [HttpPost, ActionName("UploadPicture")]
        public async Task<IActionResult> UploadPicture(int id, IFormFile file)
        {
            var home = await _context.Home
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            
            if (!CanEditHome(home))
            {
                return Unauthorized();
            }
            
            if (file != null && file.Length > 0)
            {                
                // TODO: get unique name for the file
                var fileName = $@"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fileRelativePath = Path.Combine("images", fileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileRelativePath);
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSrteam);
                }

                Picture picture = new Picture {Home = home, Filepath = "/" + fileRelativePath};
                _context.Add(picture);
                _context.SaveChanges();
                _flashMessage.Confirmation("Picture added to the home!");
                return RedirectToAction(nameof(AddPictures), new { id });
            }
            _flashMessage.Danger("We couldn't add the picture to the home!");
            return RedirectToAction(nameof(AddPictures), new { id });   
        }

        [HttpPost, ActionName("DeletePicture")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            var picture = await _context.Picture
                .Include(m => m.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }
            
            Console.WriteLine(picture);

            Home home = picture.Home;

            if (!CanEditHome(home))
            {
                return Unauthorized();
            }
            
            Console.WriteLine(home);
            _context.Remove(picture);
            _context.SaveChanges();
            
            _flashMessage.Confirmation("Picture deleted!");
            return RedirectToAction(nameof(Details), new { home.Id });
        }

        public async Task<IActionResult> UserHomes()
        {
            User user = _context.Users
                .Include(u => u.Homes)
                .ThenInclude(h => h.Pictures)
                .FirstOrDefault(u => u.UserName == User.Identity.Name);
            return View(nameof(Index), user.Homes.ToList());
        }
    }
}