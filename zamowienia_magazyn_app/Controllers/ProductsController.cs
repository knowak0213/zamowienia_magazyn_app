using Microsoft.AspNetCore.Mvc;
using zamowienia_magazyn_app.Models;

namespace zamowienia_magazyn_app.Controllers
{
    public class ProductsController : Controller
    {
        // Public static list to allow access from OrdersController (Mock Data simplified)
        public static List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Gamingowy", Description = "Wysoka wydajność, RTX 4060", Price = 4500.00m, StockQuantity = 10 },
            new Product { Id = 2, Name = "Myszka Bezprzewodowa", Description = "Ergonomiczna, Bluetooth", Price = 120.00m, StockQuantity = 50 },
            new Product { Id = 3, Name = "Monitor 4K", Description = "27 cali, IPS", Price = 1500.00m, StockQuantity = 15 },
            new Product { Id = 4, Name = "Klawiatura Mechaniczna", Description = "Przełączniki Red", Price = 350.00m, StockQuantity = 20 }
        };

        private static int _nextId = 5;

        // GET: Products
        public IActionResult Index()
        {
            return View(Products);
        }

        // GET: Products/Details/5
        public IActionResult Details(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = _nextId++;
                Products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingIndex = Products.FindIndex(p => p.Id == id);
                if (existingIndex != -1)
                {
                    Products[existingIndex] = product;
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
