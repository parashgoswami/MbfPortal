using Application.Common.Interfaces;

namespace Infrastucture.Services;

public class TimeService : ITimeService
{
    public string GetFinancialYear(DateTime date)
    {
        int startYear = date.Month >= 4 ? date.Year : date.Year - 1;
        int endYear = (startYear + 1) % 100;
        return $"{startYear}-{endYear:D2}";
    }
}
