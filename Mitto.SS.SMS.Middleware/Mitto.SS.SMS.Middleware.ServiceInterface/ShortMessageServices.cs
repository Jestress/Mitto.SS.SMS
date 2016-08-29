using Mitto.SS.SMS.Middleware.Core;
using Mitto.SS.SMS.Middleware.Core.Abstractions;
using ServiceStack;

namespace Mitto.SS.SMS.Middleware.ServiceInterface
{
    /// <summary>
    /// Service interface that configures routes and actions
    /// </summary>
    public class ShortMessageServices : Service
    {
        /// <summary>
        /// Business manager
        /// </summary>
        private readonly IMessageServicesManager manager;

        public ShortMessageServices(IMessageServicesManager managerInjection)
        {
            this.manager = managerInjection;
        }

        /// <summary>
        /// Retrieve all countries
        /// </summary>
        /// <param name="request">null</param>
        /// <returns>Country list</returns>
        public object Get(GetCountries request)
        {
            return manager.GetCountries();
        }

        /// <summary>
        /// Send sms
        /// </summary>
        /// <param name="request">SMS object that is serialized to the query string</param>
        /// <returns>Enumaration value that indicates success/fail</returns>
        public object Get(SendSms request)
        {
            return manager.SendSms(request);
        }

        /// <summary>
        /// Retrieve all sent sms records
        /// </summary>
        /// <param name="request">Object that contains fromDate, toDate, countryCode and pagination parameters</param>
        /// <returns>Sms records</returns>
        public object Get(GetSentSms request)
        {
            return manager.GetSentSms(request);
        }

        /// <summary>
        /// Get all sms statistics
        /// </summary>
        /// <param name="request">Object that contains fromDate, toDate and mobile country codes</param>
        /// <returns>Sms sendin statistics</returns>
        public object Get(GetStatistics request)
        {
            return manager.GetStatistics(request);
        }
    }
}
