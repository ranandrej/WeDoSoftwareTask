using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public static class DateUtils
    {
        public static int GetWeekOfMonth(DateTime date)
        {
            return (date.Day - 1) / 7 + 1;
        }
    }
}
