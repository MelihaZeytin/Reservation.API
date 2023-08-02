using Reservation.Entity;

namespace Reservation.DTO
{
    public class RezervasyonKontrolResponseDTO
    {
        public int Time { get; set; }
        public string Name { get; set; }


        public RezervasyonKontrolResponseDTO(ReservationEntity reservation)
        {
            Time = reservation.StartTime;
            Name = reservation.Name;

        }
    }
}
