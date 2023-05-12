using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private IWebHostEnvironment Environment;
        private PizzaContext DB;

        public PizzasController(IWebHostEnvironment _environment, PizzaContext db)
        {
            Environment = _environment;
            DB = db;
        }

        [HttpGet]
        public IActionResult GetPizzas([FromQuery] string? name)
        {
            List<Pizza> pizze;

            if (name == null || name == "")
                pizze = DB.Pizzas.ToList();
            else
                pizze = DB.Pizzas.Where(p => p.Name.ToLower().Contains(name)).ToList();

            return Ok(pizze);
        }

        [HttpGet("{id}")]
        public IActionResult GetPizza(int id)
        {
            var pizza = DB.Pizzas.FirstOrDefault(p => p.PizzaId == id);

            if (pizza is null)
            {
                return NotFound();
            }

            return Ok(pizza);
        }

        [HttpPost]
        public IActionResult CreatePizza(Pizza pizza)
        {
            DB.Pizzas.Add(pizza);
            DB.SaveChanges();

            return Ok("Pizza creata correttamente!");
        }

        [HttpPut("{id}")]
        public IActionResult PutPizza(int id, [FromBody] Pizza pizza)
        {
            var savedPizza = DB.Pizzas.FirstOrDefault(p => p.PizzaId == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            savedPizza.Name = pizza.Name;
            savedPizza.Price = pizza.Price;
            savedPizza.ImgPath = pizza.ImgPath;
            savedPizza.CategoryId = pizza.CategoryId;

            DB.SaveChanges();

            return Ok("Pizza modificata correttamente!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            var savedPizza = DB.Pizzas.FirstOrDefault(p => p.PizzaId == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            DB.Pizzas.Remove(savedPizza);
            DB.SaveChanges();

            return Ok("Pizza eliminata correttamente!");
        }
    }
}
