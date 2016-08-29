namespace Mitto.SS.SMS.Middleware.Core.Abstractions
{
    public interface IMessageServicesManager
    {
        SendSmsResponse SendSms(SendSms request);

        GetCountriesResponse GetCountries();

        GetSentSmsResponse GetSentSms(GetSentSms request);

        GetStatisticsResponse GetStatistics(GetStatistics request);
    }
}
