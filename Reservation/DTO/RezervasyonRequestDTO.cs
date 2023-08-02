using Reservation.Entity;

namespace Reservation.DTO
{
    public class RezervasyonRequestDTO
    {
        public DateTime ReservationDate { get; set; }

        public int StartTime { get; set; }

        public int Duration { get; set; }

        public string Name { get; set; }

        public ReservationEntity ToReservation()
        {

            return new ReservationEntity()
            {
                Id = 0,
                ReservationDate = this.ReservationDate,
                StartTime = this.StartTime,
                Duration = this.Duration,
                Name = this.Name
            };
        }

    }

}
