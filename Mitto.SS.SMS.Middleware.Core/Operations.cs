using ServiceStack;
using System;


/// <summary>
/// Service stack DTO's
/// </summary>
namespace Mitto.SS.SMS.Middleware.Core
{
    [Route("/sms/send")]
    public class SendSms : IReturn<SendSmsResponse>
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Text { get; set; }
    }

    public class SendSmsResponse
    {
        public SmsExecutionState State { get; set; }
    }

    [Route("/countries")]
    public class GetCountries : IReturn<GetCountriesResponse>
    {

    }

    public class GetCountriesResponse
    {
        public Country[] Countries { get; set; }
    }

    [Route("/sms/sent")]
    public class GetSentSms : IReturn<GetSentSmsResponse>
    {
        public DateTime DateTimeFrom { get; set; }

        public DateTime DateTimeTo { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

    }

    public class GetSentSmsResponse
    {
        public int TotalCount { get; set; }

        public Sms[] Items { get; set; }
    }

    [Route("/statistics")]
    public class GetStatistics : IReturn<GetStatisticsResponse>
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string[] MccList { get; set; }
    }

    public class GetStatisticsResponse
    {
        public SmsStat[] Statistics { get; set; }
    }
}