using Reservation.DatabaseContext;
using System;

namespace Reservation.Helper
{
    public class Helper
    {
        public const int workingStartTime = 9;
        public const int workingEndTime = 17;

        public readonly string[] outsideWorkingDays = {"Cumartesi", "Pazar" };

        private readonly ReservationContext _context;

        public Helper(ReservationContext context)
        {
            _context = context;
        }

        public bool IsWorkingHour(int startTime, int duration)
        {
            int endTime = startTime + duration;
            if(startTime < workingStartTime || endTime > workingEndTime)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool IsWorkingDays(DateTime reservationDate)
        {
            var result = outsideWorkingDays.Any(x => x.Equals(reservationDate.DayOfWeek));

            return !result;

        }

        public bool IsReservationConflict(DateTime reservationDate, int startTime, int duration)
        {


            for (int hour = startTime; hour <= startTime + duration; hour++)
            {
                if (_context.Reservations.Any(r => r.ReservationDate.Date == reservationDate.Date && hour >= r.StartTime && hour <= r.StartTime + r.Duration))
                {
                    return false;
                }
            }

            return true;



                          
            
        }


    }
}
