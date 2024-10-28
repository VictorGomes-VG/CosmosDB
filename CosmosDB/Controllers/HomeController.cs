using CosmosDB.Models;
using CosmosDB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CosmosDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly CosmosDbservice cosmosDbService;
        private static readonly List<string> categorias = new List<string>
        {
            "Certificação", "Gastronomia", "Tecnologia", "Saúde"
        };

        public HomeController(CosmosDbservice cosmosDbService)
        {
            this.cosmosDbService = cosmosDbService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            ViewBag.Categorias = new SelectList(categorias);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(Curso curso)
        {
            curso.id = Guid.NewGuid().ToString();
            await this.cosmosDbService.AddNewItemAsync(curso);
            return RedirectToAction("ListAllItems");
        }

        [HttpGet]
        public async Task<IActionResult> ListAllItems()
        {
            var lista = await cosmosDbService.GetAllItemsAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateItem(string id, string categoria)
        {
            ViewBag.Categorias = new SelectList(categorias);

            var curso = await cosmosDbService.FindItemAsync(id, categoria);
            return View(curso);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(Curso curso)
        {
            await cosmosDbService.UpdateItemAsync(curso);
            return RedirectToAction("ListAllItems");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveItem(string id, string categoria)
        {
            var curso = await cosmosDbService.FindItemAsync(id, categoria);
            return View(curso);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(Curso curso)
        {
            await cosmosDbService.RemoveItemAsync(curso.id!, curso.categoria!);
            return RedirectToAction("ListAllItems");
        }
    }
}
