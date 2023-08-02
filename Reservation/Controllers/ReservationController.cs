using Microsoft.AspNetCore.Mvc;
using Reservation.DatabaseContext;
using System.Reflection.Metadata;
using Reservation.Entity;
using Reservation.DTO;
using Reservation.Helper;
using Azure.Core;
using Azure;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationContext _context;
        private Helper.Helper _helper;


        public ReservationController(ReservationContext context)
        {
            _context = context;
            _helper = new Helper.Helper(_context);
        }



        [HttpPost("Reserve")]
        public async Task<IActionResult> Reserve([FromBody] RezervasyonRequestDTO rezervasyonRequest)
        {

            if(rezervasyonRequest.StartTime < 0 || rezervasyonRequest.StartTime > 23)
            {
                return BadRequest("Geçersiz başlangıç saati. Saat değeri 0-23 arasında olmalıdır.");
            }

            else if (rezervasyonRequest.Duration < 1 || rezervasyonRequest.Duration > 4)
            {
                return BadRequest("Geçersiz rezervasyon süresi. Rezervasyon süresi 1-4 saat arasında olmalıdır.");
            }

            else if (!(_helper.IsWorkingHour(rezervasyonRequest.StartTime, rezervasyonRequest.Duration) || _helper.IsWorkingDays(rezervasyonRequest.ReservationDate))) {

                return BadRequest("Mesai saatleri dışında rezervasyon yapılamaz. Mesai saatleri: Pazartesi - Cuma, 09:00 - 17:00 arası.");

            }

            else if (!_helper.IsReservationConflict(rezervasyonRequest.ReservationDate, rezervasyonRequest.StartTime, rezervasyonRequest.Duration))
            {
                return BadRequest("Seçtiğiniz saatler başka bir rezervasyonla çakışıyor. Lütfen farklı bir saat seçin.");
            }




                var item = _context.Reservations.Add(rezervasyonRequest.ToReservation());

            await _context.SaveChangesAsync();

            return Ok("Rezervasyon başarıyla yapıldı.");
        }



        [HttpPost("ReservationControl")]
        public async Task<IActionResult> ReservationControl([FromBody] RezervasyonKontrolRequestDTO request)
        {
            Dictionary<string, List<RezervasyonKontrolResponseDTO>> reservationDictionary = new Dictionary<string, List<RezervasyonKontrolResponseDTO>>();

            for (DateTime date = request.StartDate.Date; date <= request.EndDate.Date; date = date.AddDays(1))
            {
                var reservationsOnDate = _context.Reservations.Where(x => x.ReservationDate.Date == date);

                if (reservationsOnDate.Any())
                {
                    List<RezervasyonKontrolResponseDTO> dateReservationList = new List<RezervasyonKontrolResponseDTO>();

                    foreach (var reservation in reservationsOnDate)
                    {
                            dateReservationList.Add(new RezervasyonKontrolResponseDTO(reservation));
                        
                    }
                    var sortedList = dateReservationList.OrderBy(response => response.Time).ToList();
                    reservationDictionary.Add(date.ToString().Substring(0, 10), sortedList);
                }
            }

            return Ok(reservationDictionary);
        }




        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var item = _context.Reservations.FirstOrDefault(a => a.Id == id);

            if (item == null)
                return NotFound();
            else
            {
                _context.Reservations.Remove(item);
                _context.SaveChanges();
                return Ok();
            }

        }
    }
}
