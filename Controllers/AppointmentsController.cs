using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Barbershop_booking.Data;
using Barbershop_booking.Models;

namespace Barbershop_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMyAppointments()
        {
            var userId = 2; // for noww

            var appointments = _context.Appointments
                .Include(a => a.Barber)
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.DateTime)
                .ToList();

            return Ok(appointments);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll([FromQuery] int? barberId, [FromQuery] DateTime? date)
        {
            var query = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Barber)
                .Include(a => a.Service)
                .AsQueryable();

            if (barberId.HasValue)
                query = query.Where(a => a.BarberId == barberId.Value);

            if (date.HasValue)
                query = query.Where(a => a.DateTime.Date == date.Value.Date);

            return Ok(query.OrderBy(a => a.DateTime).ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAppointmentDto dto)
        {
            var userId = 2;

            if (dto.DateTime < DateTime.Now)
                return BadRequest("No se pueden reservar citas en el pasado.");

            var barber = _context.Barbers.Find(dto.BarberId);
            if (barber == null || !barber.IsActive)
                return BadRequest("Barbero no disponible.");

            var service = _context.Services.Find(dto.ServiceId);
            if (service == null)
                return BadRequest("Servicio no encontrado.");

            // disponibilidad
            var endTime = dto.DateTime.AddMinutes(service.DurationMinutes);
            var conflict = _context.Appointments.Any(a =>
                a.BarberId == dto.BarberId &&
                a.Status != "Cancelada" &&
                a.DateTime < endTime &&
                a.DateTime.AddMinutes(
                    _context.Services.First(s => s.Id == a.ServiceId).DurationMinutes
                ) > dto.DateTime);

            if (conflict)
                return BadRequest("El barbero no está disponible en ese horario.");

            var appointment = new Appointment
            {
                UserId = userId,
                BarberId = dto.BarberId,
                ServiceId = dto.ServiceId,
                DateTime = dto.DateTime,
                Status = "Confirmada"
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return Ok("Cita agendada exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            var userId = 2;
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id && a.UserId == userId);

            if (appointment == null) return NotFound();

            if (appointment.DateTime.AddHours(-2) < DateTime.Now)
                return BadRequest("Solo se puede modificar con al menos 2 horas de antelación.");

            if (dto.DateTime < DateTime.Now)
                return BadRequest("No se pueden reservar citas en el pasado.");

            appointment.DateTime = dto.DateTime;
            _context.SaveChanges();

            return Ok("Cita modificada exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var userId = 2;
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id && a.UserId == userId);

            if (appointment == null) return NotFound();

            if (appointment.DateTime.AddHours(-2) < DateTime.Now)
                return BadRequest("Solo se puede cancelar con al menos 2 horas de antelación.");

            appointment.Status = "Cancelada";
            _context.SaveChanges();

            return Ok("Cita cancelada exitosamente.");
        }
    }

    public class CreateAppointmentDto
    {
        public int BarberId { get; set; }
        public int ServiceId { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class UpdateAppointmentDto
    {
        public DateTime DateTime { get; set; }
    }
}