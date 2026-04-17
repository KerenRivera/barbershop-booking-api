namespace Barbershop_booking.Models
{
    public class Barber
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialties { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
