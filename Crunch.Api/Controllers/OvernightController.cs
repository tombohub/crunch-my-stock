using Crunch.Api.Dtos;
using Crunch.Database;
using Crunch.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crunch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvernightController : ControllerBase
    {
        private readonly stock_analyticsContext _db;
        public OvernightController(stock_analyticsContext ctx)
        {
            _db = ctx;
        }

        /// <summary>
        /// Last recorded date for overnight prices data
        /// </summary>
        /// <returns></returns>
        [HttpGet("last-recorded-date")]
        public ActionResult<DateOnly> LastRecordedDate()
        {
            var dbMethods = new DatabaseMethods();
            DateOnly lastDate = dbMethods.GetLastRecordedOvernightDate();
            return Ok(lastDate);
        }

        [HttpGet("average-roi")]
        public ActionResult<AverageRoiDTO> AverageRoi(DateOnly? date)
        {
            var dbMethods = new DatabaseMethods();
            decimal avgRoi;

            if (date == null)
            {
                //NOTE: mutating date
                DateOnly today = dbMethods.GetLastRecordedOvernightDate();
                avgRoi = dbMethods.GetAverageRoi(today);
            }

            else
            {
                avgRoi = dbMethods.GetAverageRoi(date.Value);
            }

            return Ok(avgRoi);

        }

        /// <summary>
        /// Winners and losers count from overnight performance on a given day
        /// </summary>
        /// <returns></returns>
        [HttpGet("winners-losers-count")]
        public ActionResult<WinnersLosersCountDTO> WinnersLosersCount(DateOnly? date)
        {

            if (date == null)
            {
                var dbMethods = new DatabaseMethods();

                //NOTE: mutating date
                date = dbMethods.GetLastRecordedOvernightDate();
            }

            var winnersCount = _db.DailyOvernightPerformances
                .Count(x => x.Date == date && x.ChangePct > 0);

            var losersCount = _db.DailyOvernightPerformances
                .Count(x => x.Date == date && x.ChangePct < 0);

            var dto = new WinnersLosersCountDTO
            {
                Date = date.Value,
                WinnersCount = winnersCount,
                LosersCount = losersCount
            };

            return Ok(dto);
        }
    }
}
