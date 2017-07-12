using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class ArchiveEntry
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName
        {
            get
            {
                return CultureInfo.GetCultureInfo("en").DateTimeFormat.GetMonthName(this.Month);
            }
        }
        public int Total { get; set; }
    }
}