using CosmosDB.Models;
using CosmosDB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CosmosDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly CosmosDbservice cosmosDbservice;
        public HomeController(CosmosDbservice cosmosDbservice)
        {
            this.cosmosDbservice = cosmosDbservice;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            List<string> categorias = new List<string>
            {
                "Certificação", "Gastronomia", "Tecnologia", "Saude"
            };

            ViewBag.Categorias = new SelectList(categorias);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(Curso curso)
        {
            curso.id = Guid.NewGuid().ToString();
            await this.cosmosDbservice.AddNewItemAsync(curso);
            return RedirectToAction("ListAllItems");
        }

        [HttpGet]
        public IActionResult ListAllItems()
        {
            return View();
        }
    }
}
