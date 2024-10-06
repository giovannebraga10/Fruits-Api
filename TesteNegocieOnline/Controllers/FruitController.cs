using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TesteNegocieOnline.Models;
using TesteNegocieOnline.Repositories;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TesteNegocieOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        HttpClient client = new HttpClient(); // criação do client http para consumir apis
        FruitRepository repository; // atributo para receber o repository

        public FruitController(FruitRepository fruitRepository) // Construtor que recebe o repository via injeção de dependencia
        { 
            repository = fruitRepository; // exporta o repository injetado para o atributo da classe
        }

        // GET: api/<FruitController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var fruits = await repository.SelectAllFruits();
            return Ok(fruits);
        }

       
        // GET api/<FruitController>/5
        [HttpPost("{idfruit}")]
        public async Task<IActionResult> Get(int idfruit)
        {
            var response = await client.GetAsync($"https://www.fruityvice.com/api/fruit/{idfruit}"); // método assincrono para fazer requisições
            var json = await response.Content.ReadAsStringAsync(); // método para ler o conteúdo da resposta da requisição em string
            var fruit = JsonConvert.DeserializeObject<Fruit>(json); // método para desserializar a string do conteúdo da resposta para o objeto model

            if(fruit.id == 0)
            {
                return NotFound();
            }

            await repository.InsertFruit(fruit);

            return Ok();
        }

    }
}
