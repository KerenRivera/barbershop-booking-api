using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Barbershop_booking.Data;
using Barbershop_booking.Models;

namespace Barbershop_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var services = _context.Services.ToList();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Service updated)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();

            service.Name = updated.Name;
            service.Description = updated.Description;
            service.Price = updated.Price;
            service.DurationMinutes = updated.DurationMinutes;

            _context.SaveChanges();
            return Ok(service);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();

            _context.Services.Remove(service);
            _context.SaveChanges();
            return NoContent();
        }
    }
}