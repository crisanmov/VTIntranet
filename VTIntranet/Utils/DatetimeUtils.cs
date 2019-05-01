using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VTIntranet.Utils
{
    public class DatetimeUtils
    {
        public static DateTime? ParseStringToDate(string datetime)
        {

            DateTime? date = null;
            if (string.IsNullOrEmpty(datetime))
                date = null;
            else date = DateTime.ParseExact(datetime, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            return date;

        }

        public static string ParseDatetoString(DateTime? datetime)
        {
            if (datetime == null)
            { return ""; }
            else { return string.Format("{0:dd/MM/yyyy}", datetime); }
        }

        public static string ParseDatetoStringSingle(DateTime? datetime)
        {
            if (datetime == null)
            { return ""; }
            else { return string.Format("{0:yyyyMMddHHmmss}", datetime); }
        }

        // Retorna el dia de la semana en entero de la fecha seleccionada
        public static int dayOfWeek(DateTime date)
        {
            CultureInfo cli = CultureInfo.InvariantCulture;
            int dayOfWeek = (int)cli.Calendar.GetDayOfWeek(date);
            //DateTime currentdate = date.Subtract(new TimeSpan(WeekDays[dayOfWeek], 0, 0, 0));
            return dayOfWeek;
        }
        // retorna el día del mes de la fecha seleccionada
        public static int dayOfMonth(DateTime date)
        {
            CultureInfo cli = CultureInfo.InvariantCulture;
            int dayOfWeek = (int)cli.Calendar.GetDayOfMonth(date);
            //DateTime currentdate = date.Subtract(new TimeSpan(WeekDays[dayOfWeek], 0, 0, 0));
            return dayOfWeek;
        }
        // retorna el mes en entero de la fecha seleccionada
        public static int MonthOfDate(DateTime date)
        {
            CultureInfo cli = CultureInfo.InvariantCulture;
            int numberMoth = (int)cli.Calendar.GetMonth(date);
            //DateTime currentdate = date.Subtract(new TimeSpan(WeekDays[dayOfWeek], 0, 0, 0));
            return numberMoth;
        }
        // retorna el numero de días del mes de la fecha seleccionada
        public static int TotalDaysMonth(DateTime date)
        {
            CultureInfo cli = CultureInfo.InvariantCulture;
            int numberDays = (int)cli.Calendar.GetDaysInMonth(date.Year, date.Month);
            //DateTime currentdate = date.Subtract(new TimeSpan(WeekDays[dayOfWeek], 0, 0, 0));
            return numberDays;
        }
        // Retorna el mes mes de la fecha seleccionada en Español
        public static string getMonthNameEs(DateTime date)
        {
            string fullMonthName = date.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
            return fullMonthName;
        }
    }
}