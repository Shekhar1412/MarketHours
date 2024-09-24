namespace MarketHours

{
public interface IMarketHourService
{
    public int MarketTime(DateTime dateTimeParam);
    public int CalculateSecondsForDay(DateTime dateTimeParam);
    public int CalculateTotalWorkDays(DateTime StartDate, DateTime EndDate, List<int> holidays);
    public bool IsMarketDay(DateTime dateTimeParam, List<int> holidays);
    public int CalculateSecondsForDayPast(DateTime dateTimeParam);
}
}

