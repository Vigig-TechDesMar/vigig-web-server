namespace Vigig.Common.Helpers;

public class AverageHelper
{
    public static double GetAverage(double currentAverage, int totalCount, double newValue)
    {
        return (currentAverage * totalCount + newValue) / (totalCount + 1);
    }
}