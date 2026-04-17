namespace Barbershop_booking.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; } = "Confirmada"; // Confirmada, Cancelada, Completada

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int BarberId { get; set; }
        public Barber Barber { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
