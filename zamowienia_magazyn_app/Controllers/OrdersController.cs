using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using zamowienia_magazyn_app.Models;

namespace zamowienia_magazyn_app.Controllers
{
    public class OrdersController : Controller
    {
        public static List<Order> Orders = new List<Order>();
        private static int _nextId = 1;

        public IActionResult Index()
        {
            return View(Orders.OrderByDescending(o => o.OrderDate).ToList());
        }

        public IActionResult Details(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult Create()
        {
            ViewBag.ClientId = new SelectList(ClientsController.Clients, "Id", "FullName");
            ViewBag.Products = ProductsController.Products;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int clientId, Dictionary<int, int> productQuantities)
        {
            // Simple validation
            if (clientId == 0)
            {
                ModelState.AddModelError("ClientId", "Wybierz klienta.");
            }
            
            var selectedProducts = productQuantities?.Where(x => x.Value > 0).ToList();
            if (selectedProducts == null || !selectedProducts.Any())
            {
                ModelState.AddModelError("", "Wybierz przynajmniej jeden produkt.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ClientId = new SelectList(ClientsController.Clients, "Id", "FullName", clientId);
                ViewBag.Products = ProductsController.Products;
                return View();
            }

            var client = ClientsController.Clients.FirstOrDefault(c => c.Id == clientId);
            if (client == null) return NotFound();

            var order = new Order
            {
                Id = _nextId++,
                OrderDate = DateTime.Now,
                ClientId = clientId,
                Client = client,
                Status = OrderStatus.New,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in selectedProducts)
            {
                var product = ProductsController.Products.FirstOrDefault(p => p.Id == item.Key);
                if (product != null)
                {
                    // Basic stock check could go here
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = item.Value,
                        UnitPrice = product.Price
                    });
                }
            }

            Orders.Add(order);
            return RedirectToAction(nameof(Index));
        }

        // Keep it simple for now - no Edit/Delete for Orders in this phase
    }
}
