namespace Reservation.Entity
{
    public class ReservationEntity : BaseEntity
    {
        public DateTime ReservationDate { get;set;}

        public int StartTime { get;set;}

        public int Duration { get;set;}

        public string Name { get;set;}
    }
}
