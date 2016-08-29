using Mitto.SS.SMS.Middleware.Core;
using Mitto.SS.SMS.Middleware.Core.Abstractions;
using System;
using System.Text.RegularExpressions;

namespace Mitto.SS.SMS.Business
{
    /// <summary>
    /// Business manager class that performs business operations between Service interface and database
    /// </summary>
    public class MessageServicesManager : IMessageServicesManager
    {
        /// <summary>
        /// Regular expression pattern for extracting country code from phone number in query string
        /// </summary>
        private static readonly string MobileCountryCodeRegexPattern = "[+]{1}([0-9]{2})";

        /// <summary>
        /// Default SMS data provider
        /// </summary>
        private readonly ISmsEntitiesDataProvider dataProvider;

        public MessageServicesManager(ISmsEntitiesDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Get all defined countries on the system
        /// </summary>
        /// <returns>List of countries wrapper with a response DTO</returns>
        public GetCountriesResponse GetCountries()
        {
            var response = new GetCountriesResponse();

            response.Countries = dataProvider.RetrieveCountries();

            return response;
        }

        /// <summary>
        /// Retrieves all sent sms entities on database, according to the request DTO parameters
        /// </summary>
        /// <param name="request">Contains filtering parameters that is mapped from service interface. Such as sms start date, end date etc.</param>
        /// <returns>Returns a collection of SMS entities wrapped with a response DTO</returns>
        public GetSentSmsResponse GetSentSms(GetSentSms request)
        {
            var response = new GetSentSmsResponse();
            var totalCount = 0;

            response.Items = dataProvider.GetSentSms(request.DateTimeFrom, request.DateTimeTo, request.Skip, request.Take, out totalCount);

            response.TotalCount = totalCount;
            return response;
        }

        /// <summary>
        /// Retrieves sms communication statistics.
        /// </summary>
        /// <param name="request">Contains filtering parameters which will be performed on sms entities to build a statistics result</param>
        /// <returns>Returns a list of <see cref="SmsStat"/> that contains statistics</returns>
        public GetStatisticsResponse GetStatistics(GetStatistics request)
        {
            var smsStats = dataProvider.RetrieveStatistics(request.DateFrom, request.DateTo, request.MccList);

            return new GetStatisticsResponse { Statistics = smsStats };
        }

        /// <summary>
        /// Sends an sms to the target number, with requested text content.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns a response DTO that contains success/fails information about the insertion operation.</returns>
        public SendSmsResponse SendSms(SendSms request)
        {
            var sms = new Sms
            {
                From = request.From,
                To = request.To,
                CountryCode = ExtractMobileCountryCode(request.To),
                Text = request.Text,
                SentDate = DateTime.Now,
                State = SmsExecutionState.Success
            };

            var result = dataProvider.SendSms(sms);

            return new SendSmsResponse { State = (result) ? SmsExecutionState.Success : SmsExecutionState.Fail };
        }

        /// <summary>
        /// Extracts mobile country code by trying to match a regex pattern with the given string
        /// </summary>
        /// <param name="to">To number field of the SMS entity to be parsed</param>
        /// <returns>If regex matches successfully, returns 2 digit country code</returns>
        private static int ExtractMobileCountryCode(string to)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new Exception("Target phone number is empty!");
            }

            var match = Regex.Match(to, MobileCountryCodeRegexPattern);

            if(match == null)
            {
                throw new Exception("No suitable country code information could be retrieved.");
            }

            return match.Value.ToInt();
        }
    }
}
