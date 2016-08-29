using System;

namespace Mitto.SS.SMS.Middleware.Core.Abstractions
{
    public interface ISmsEntitiesDataProvider
    {
        Country[] RetrieveCountries();

        SmsStat[] RetrieveStatistics(DateTime from, DateTime to, string[] mobileCodes);

        bool SendSms(Sms sms);

        Sms[] GetSentSms(DateTime from, DateTime to, int skip, int take, out int totalCount);
    }
}
