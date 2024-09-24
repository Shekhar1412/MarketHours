using MarketHours;
using MarketHours.Enum;
using MarketHoursApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketHoursApp.Controllers
{
    
    
    [Route("api/[controller]")]
    [ApiController]    
    public class MarketHoursController :ControllerBase
    {
        private readonly IMarketHourService _marketHours;
        private readonly IConfiguration _config;

        public MarketHoursController(IMarketHourService marketHourService,IConfiguration configuration)
        {
            _config = configuration;
            _marketHours = marketHourService;
        }
        [HttpPost]
        public IActionResult Get(MarketHoursRequest request)
        {
            var marketStart = DateTime.Parse(_config.GetValue<string>("MarketStart"));
            var totalDaySeconds = _marketHours.CalculateSecondsForDay(marketStart);
            int secondsForToday=0;
            int marketState =_marketHours.MarketTime(request.StartDate);
            if(_marketHours.IsMarketDay(request.StartDate,request.Holidays) && (marketState == (int)MarketTimings.EARLY || marketState ==  (int)MarketTimings.INTIME ) )
            {
                secondsForToday = _marketHours.CalculateSecondsForDay(request.StartDate);
            }
            int secondsForEndDay = _marketHours.CalculateSecondsForDayPast(request.EndDate);
            DateTime newStartDate = new DateTime(request.StartDate.Year,request.StartDate.Month,request.StartDate.AddDays(1).Day, marketStart.Hour,marketStart.Minute,marketStart.Second);
            DateTime newEndDate = new DateTime(request.EndDate.Year,request.EndDate.Month,request.EndDate.Day, marketStart.Hour,marketStart.Minute,marketStart.Second);

            int workDays = _marketHours.CalculateTotalWorkDays(newStartDate,newEndDate,request.Holidays);
            double result = (double)secondsForToday + workDays*totalDaySeconds+ secondsForEndDay;
            return Ok(String.Format("{0:0.00}", (double)result/3600));
        }
    }
}


