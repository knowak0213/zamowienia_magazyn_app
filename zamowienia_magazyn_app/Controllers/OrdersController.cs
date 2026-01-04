using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using zamowienia_magazyn_app.Data;
using zamowienia_magazyn_app.Models;

namespace zamowienia_magazyn_app.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var orders = await _context.Orders.Include(o => o.Client).OrderByDescending(o => o.OrderDate).ToListAsync();
                return View(orders);
            }
            else
            {
                var orders = await _context.Orders
                    .Include(o => o.Client)
                    .Where(o => o.UserId == user.Id)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
                return View(orders);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();

            // Authorization check for details
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (order.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            return View(order);
        }

        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "LastName");
            ViewBag.Products = _context.Products.Where(p => p.StockQuantity > 0).ToList(); // Only show available products
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int clientId, Dictionary<int, int> productQuantities)
        {
            if (clientId == 0)
            {
                ModelState.AddModelError("ClientId", "Wybierz klienta.");
            }
            
            var selectedProducts = productQuantities?.Where(x => x.Value > 0).ToList();
            if (selectedProducts == null || !selectedProducts.Any())
            {
                ModelState.AddModelError("", "Wybierz przynajmniej jeden produkt.");
            }

            // Database validation for Client
            var client = await _context.Clients.FindAsync(clientId);
            if (client == null) return NotFound();

            // Stock validation
            foreach (var item in selectedProducts)
            {
                var productToCheck = await _context.Products.FindAsync(item.Key);
                if (productToCheck == null) continue;
                
                if (productToCheck.StockQuantity < item.Value)
                {
                     ModelState.AddModelError("", $"Niewystarczająca ilość towaru: {productToCheck.Name} (Dostępne: {productToCheck.StockQuantity})");
                }
            }

            if (!ModelState.IsValid)
            {
                 ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "LastName", clientId);
                 ViewBag.Products = _context.Products.Where(p => p.StockQuantity > 0).ToList();
                return View();
            }

            var user = await _userManager.GetUserAsync(User);

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ClientId = clientId,
                Client = client,
                UserId = user?.Id,
                Status = OrderStatus.New,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in selectedProducts)
            {
                var product = await _context.Products.FindAsync(item.Key);
                if (product != null)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = item.Value,
                        UnitPrice = product.Price
                    });

                    // Update Stock
                    product.StockQuantity -= item.Value;
                    _context.Update(product);
                }
            }

            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] Order order)
        {
            if (id != order.Id) return NotFound();

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null) return NotFound();

            // Only update status, keep other fields
            existingOrder.Status = order.Status;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(existingOrder);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
