using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class UnitTest
    {
        public string Series { get; set; }
        public int Number { get; set; }
        public string ExpectedResult { get; set; }
        public string ActualResult { get; set; }
        public DateTime DateLastRun { get; set; }
        public bool? CorrectFlag { get; set; }
    }
}
