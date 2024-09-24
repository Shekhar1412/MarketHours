using System;
using System.ComponentModel;
using MarketHours.Enum;

namespace MarketHours.Services
{
public class MarketHourService:IMarketHourService
{
    private readonly IConfiguration _config;
    //public static List<int> _holidays;//0 = Sunday

    public MarketHourService(IConfiguration configuration)
    {
        _config = configuration;
    }
    public int MarketTime(DateTime dateTimeParam)
    {
        TimeOnly marketStart = TimeOnly.FromDateTime(DateTime.Parse(_config.GetValue<string>("MarketStart")));
        TimeOnly marketEnd = TimeOnly.FromDateTime(DateTime.Parse(_config.GetValue<string>("MarketEnd")));

        TimeOnly currentTime = TimeOnly.FromDateTime(dateTimeParam);
        int result;
        if(currentTime < marketStart)
        {
            result = (int)MarketTimings.EARLY;
        }
        else if(currentTime > marketStart && currentTime < marketEnd)
        {
            result = (int)MarketTimings.INTIME;
        }
        else{
            result = (int)MarketTimings.LATER;
        }
        return result;
    }

    public bool IsMarketDay(DateTime dateTimeParam, List<int> holidays)
    {
        int weekdayNumber = (int)dateTimeParam.DayOfWeek;
        var result = true;
        if(holidays.Contains(weekdayNumber))
        {
            result = false;
        }
        return result;
    }
    public int CalculateSecondsForDay(DateTime dateTimeParam)
    {
        //DateTime marketStart = DateTime.Parse(_config.GetValue<string>("MarketStart"));
        //DateTime marketEnd = DateTime.Parse(_config.GetValue<string>("MarketEnd"));
        TimeOnly marketStart = TimeOnly.FromDateTime(DateTime.Parse(_config.GetValue<string>("MarketStart")));
        TimeOnly marketEnd = TimeOnly.FromDateTime(DateTime.Parse(_config.GetValue<string>("MarketEnd")));
        TimeOnly currentTime = TimeOnly.FromDateTime(dateTimeParam);
        var result = 0;
        if(currentTime > marketStart)
        {
            result = (int)(marketEnd-currentTime).TotalSeconds ;
        }
        else
        {
            result =(int)(marketEnd-marketStart).TotalSeconds ;
        } 
        return result;
    }
    public int CalculateSecondsForDayPast(DateTime dateTimeParam)
    {
        TimeOnly marketStart = TimeOnly.FromDateTime(DateTime.Parse(_config.GetValue<string>("MarketStart")));
        TimeOnly currentTime = TimeOnly.FromDateTime(dateTimeParam);
        var result = 0;
        if(currentTime > marketStart)
        {
            result = (int)(currentTime-marketStart).TotalSeconds ;
        }
        return result;
    }
    public int CalculateTotalWorkDays(DateTime StartDate, DateTime EndDate, List<int> holidays)
    {
        int[] weekDays = new int[13];
        foreach (var item in holidays)
        {
            weekDays[item] = 1;
            if(item+7 < 13)
            {
                weekDays[item +7] = 1;
            }
        }
        //EndDate = EndDate.AddDays(1);
        int absoluteWorkDays = ((int)(EndDate  - StartDate).Days );
        absoluteWorkDays = absoluteWorkDays/7;
        absoluteWorkDays =absoluteWorkDays *(7-holidays.Count);
        int remainderDays =  (int)(EndDate  - StartDate).TotalDays % 7;
        var countFromRemainderDays = 0;
        if(remainderDays > 0)
        {
            int startdayNumber = (int)StartDate.DayOfWeek;
            for(int i =startdayNumber; i<startdayNumber + remainderDays; i++)
            {
                if(weekDays[i] ==0)
                {
                    countFromRemainderDays ++;
                }
            }
        }
        var result  = absoluteWorkDays + countFromRemainderDays;
        return result;
    }


}

}

