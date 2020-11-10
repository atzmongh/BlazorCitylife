using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class ErrorCode
    {
        public ErrorCode()
        {
            ErrorMessages = new HashSet<ErrorMessage>();
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public int OccurenceCount { get; set; }
        public DateTime LastOccurenceDate { get; set; }

        public virtual ICollection<ErrorMessage> ErrorMessages { get; set; }
    }
}
