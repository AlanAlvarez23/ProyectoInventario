using Inventario.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Inventario.Frontend.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7041/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Productos");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var productos = JsonConvert.DeserializeObject<IEnumerable<Producto>>(content);
                return View("Index", productos);
            }
            return View(new List<Producto>());
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Productos/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Producto creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al crear el producto!!!";
                }
            }
            return View(producto);
        }
    }
}
