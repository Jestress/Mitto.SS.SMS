using System;

namespace Mitto.SS.SMS.Middleware.Core
{
    public class Sms
    {
        public DateTime SentDate { get; set; }

        public string From { get; set; }

        public string Text { get; set; }

        public string To { get; set; }

        public int CountryCode { get; set; }

        public decimal Price { get; set; }

        public SmsExecutionState State { get; set; }
    }

    public enum SmsExecutionState
    {
        Success = 0,
        Fail,
    }
}
