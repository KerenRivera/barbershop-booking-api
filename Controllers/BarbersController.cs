using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Barbershop_booking.Data;
using Barbershop_booking.Models;

namespace Barbershop_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarbersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BarbersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var barbers = _context.Barbers.Where(b => b.IsActive).ToList();
            return Ok(barbers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var barber = _context.Barbers.Find(id);
            if (barber == null) return NotFound();
            return Ok(barber);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Barber barber)
        {
            _context.Barbers.Add(barber);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = barber.Id }, barber);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Barber updated)
        {
            var barber = _context.Barbers.Find(id);
            if (barber == null) return NotFound();

            barber.Name = updated.Name;
            barber.Specialties = updated.Specialties;
            barber.Phone = updated.Phone;
            barber.IsActive = updated.IsActive;

            _context.SaveChanges();
            return Ok(barber);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var barber = _context.Barbers.Find(id);
            if (barber == null) return NotFound();

            barber.IsActive = false;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
