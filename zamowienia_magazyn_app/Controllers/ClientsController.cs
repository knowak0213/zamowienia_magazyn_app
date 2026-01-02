using Microsoft.AspNetCore.Mvc;
using zamowienia_magazyn_app.Models;

namespace zamowienia_magazyn_app.Controllers
{
    public class ClientsController : Controller
    {
        // Public static list for Mock Data
        public static List<Client> Clients = new List<Client>
        {
            new Client { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com", PhoneNumber = "123-456-789", Address = "Warszawa, ul. Złota 44" },
            new Client { Id = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com", PhoneNumber = "987-654-321", Address = "Kraków, Rynek Główny 1" },
            new Client { Id = 3, FirstName = "Piotr", LastName = "Zieliński", Email = "p.zielinski@example.com", PhoneNumber = "555-666-777", Address = "Gdańsk, ul. Długa 5" }
        };

        private static int _nextId = 4;

        public IActionResult Index()
        {
            return View(Clients);
        }

        public IActionResult Details(int id)
        {
            var client = Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                client.Id = _nextId++;
                Clients.Add(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public IActionResult Edit(int id)
        {
            var client = Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Client client)
        {
            if (id != client.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var index = Clients.FindIndex(c => c.Id == id);
                if (index != -1)
                {
                    Clients[index] = client;
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(client);
        }

        public IActionResult Delete(int id)
        {
            var client = Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = Clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                Clients.Remove(client);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
