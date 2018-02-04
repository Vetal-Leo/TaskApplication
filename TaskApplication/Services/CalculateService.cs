using System;
using System.Collections.Generic;
using System.Linq;
using TaskApplication.Models;

namespace TaskApplication.Services
{
    //This is a separate Calculate service.
    public class CalculateService : ICalculate
    {
        private readonly ApplicationContext _context;
        public List<Emploees> Emploees { get; set; }

        public CalculateService(ApplicationContext database)
        {
            _context = database;
        }

        public CalculateService()
        {
        }

        //*This method is written like this, because the condition of the job requires it,
        //but the more correct solution is to find the employee in the database by ID.
        //This is a method for calculating the salary of non-fixed employees.
        public int Consider(string firstname, string lastname)
        {
            var emploee = _context.Emploees.FirstOrDefault(e => e.F_NAME == firstname && e.L_NAME == lastname);
            return Convert.ToInt32(emploee.COUNT_HOUR) * emploee.RATE;
        }

        //This is a method for calculating the salary of fixed employees.
        public int[] ConsiderFix(string firstname, string lastname, DateTime FirstDate, DateTime LastDate)
        {
            var workdays = WorkdaysCounter(FirstDate, LastDate);
            var emploee = _context.Emploees.FirstOrDefault(e => e.F_NAME == firstname && e.L_NAME == lastname);
            if (emploee.COUNT_DAYS < workdays) { workdays = Convert.ToInt32(emploee.COUNT_DAYS); }

            return new int[] { workdays * emploee.RATE, workdays };
        }

        //It is a counter of working days in a time interval.
        private static int WorkdaysCounter(DateTime FirstDate, DateTime LastDate)
        {
            int workdays = 0;
            DateTime tempdate = FirstDate;
            while (tempdate <= LastDate)
            {
                tempdate = tempdate.AddDays(1);
                if (tempdate.DayOfWeek != DayOfWeek.Saturday && tempdate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workdays++;
                }
            }
            return workdays;
        }
    }
}
