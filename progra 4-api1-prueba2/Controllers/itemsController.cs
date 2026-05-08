using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace progra_4_api1_prueba2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class itemsController : ControllerBase
    {
        private static readonly List<Item> _items = new()
        {
            new Item(1, "Ejemplo", "Item inicial")
        };
        private static int _nextId = 2;
        private static readonly object _lock = new();

        // GET api/items
        [HttpGet]
        public ActionResult<IEnumerable<ItemDto>> GetAll()
            => Ok(_items.Select(i => new ItemDto(i.Id, i.Name, i.Description)));

        // GET api/items/{id}
        [HttpGet("{id:int}")]
        public ActionResult<ItemDto> Get(int id)
        {
            var it = _items.FirstOrDefault(x => x.Id == id);
            if (it is null) return NotFound();
            return Ok(new ItemDto(it.Id, it.Name, it.Description));
        }

        // POST api/items
        [HttpPost]
        public ActionResult<ItemDto> Create([FromBody] CreateItemDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int id;
            lock (_lock)
            {
                id = _nextId++;
                _items.Add(new Item(id, input.Name, input.Description));
            }

            var dto = new ItemDto(id, input.Name, input.Description);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // PUT api/items/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateItemDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            lock (_lock)
            {
                var it = _items.FirstOrDefault(x => x.Id == id);
                if (it is null) return NotFound();
                it.Name = input.Name;
                it.Description = input.Description;
            }

            return NoContent();
        }

        // DELETE api/items/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            lock (_lock)
            {
                var it = _items.FirstOrDefault(x => x.Id == id);
                if (it is null) return NotFound();
                _items.Remove(it);
            }
            return NoContent();
        }

  
        private sealed class Item
        {
            public Item(int id, string name, string? description)
            {
                Id = id;
                Name = name;
                Description = description;
            }

            public int Id { get; }
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
        }

        public sealed record ItemDto(int Id, string Name, string? Description);

        public sealed record CreateItemDto
        {
            [Required]
            public string Name { get; init; } = null!;
            public string? Description { get; init; }
        }

        public sealed record UpdateItemDto
        {
            [Required]
            public string Name { get; init; } = null!;
            public string? Description { get; init; }
        }
    }
}
