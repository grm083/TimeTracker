using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models.Common
{
    public static class Services
    {
        //public static string URl = "/SBS.IT.Utilities.TimeTrackerWebAPI";

        public static string GetAllApplication = "api/Application/GetApplication";
        public static string GetProjectItem = "api/Project/GetProject?applicationId=";
        public static string GetAllWorkType = "api/WorkType/GetWorkType";


        public static List<Month> GetMonthsForDropDown()
        {
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            List<Month> lstMonths = new List<Month>();
            for (int i = 0; i < (months.Length - 1); i++)
            {
                lstMonths.Add(new Month
                {
                    MonthId = i + 1,
                    MonthName = months[i]
                });
            }
            return lstMonths;
        }

        public static List<Year> GetYearsForDropDown()
        {
            int year = DateTime.Today.Year;
            List<Year> lstYears = new List<Year>();
            for (int i = 2017; i <= year; i++)
            {
                lstYears.Add(new Year
                {
                    YearId = i,
                    YearName = i.ToString()
                });
            }
            return lstYears.OrderByDescending(x => x.YearId).ToList();
        }
    }

    public enum eNotificationType
    {
        eSuccess,
        eError
    }
}