using System;

namespace Mitto.SS.SMS.Middleware.Core
{
    public class SmsStat
    {
        public DateTime Day { get; set; }

        public string MobileCountryCode { get; set; }
        
        public decimal PricePerSms { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
