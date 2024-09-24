namespace MarketHoursApp.Models
{
    public class MarketHoursRequest
    {
        public List<int> Holidays {get; set;}
        public DateTime StartDate {get; set;}
        public DateTime EndDate {get; set;}

    }
}