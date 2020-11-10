using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class ErrorMessage
    {
        public int Id { get; set; }
        public string FormattedMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime LastOccurenceDate { get; set; }
        public int OccurenceCount { get; set; }
        public int ErrorCodeCode { get; set; }

        public virtual ErrorCode ErrorCodeCodeNavigation { get; set; }
    }
}
