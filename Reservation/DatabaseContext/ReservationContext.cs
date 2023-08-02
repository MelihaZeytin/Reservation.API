using Microsoft.EntityFrameworkCore;
using Reservation.Entity;

namespace Reservation.DatabaseContext
{
    public class ReservationContext : DbContext
    {
        public DbSet<ReservationEntity> Reservations { get; set; }

        public ReservationContext(DbContextOptions options) : base(options)
        {

        }
    }
}
