
using System;
using System.Collections.Generic;
using System.Linq;
using TaskApplication.Models;

namespace TaskApplication.Services
{

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

        public int Consider(string firstname, string lastname)
        {
            int resalt;
            var emploee = _context.Emploees.FirstOrDefault(e => e.F_NAME == firstname && e.L_NAME == lastname);
            if (emploee.FIX_PAYMENT)
            {
                resalt = Convert.ToInt32(emploee.COUNT_DAYS) * emploee.RATE;
            }
            else
            {
                resalt = Convert.ToInt32(emploee.COUNT_HOUR) * emploee.RATE;
            }

            return resalt;
        }

        public int AmountWorkDays()
        {
            return 1;
        }
    }
}
