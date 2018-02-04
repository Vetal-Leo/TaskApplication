using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskApplication.Models;
using TaskApplication.Services;
using TaskApplication.Validations;

namespace TaskApplication.Pages
{
    public class IndexModel : PageModel
    {

        private CalculateService _calculate;
        private readonly ApplicationContext _context;
        public List<Emploees> Emploees { get; set; }
        public ArrayList CalculateResult;

        public IndexModel(ApplicationContext database)
        {
            _context = database;
            _calculate = new CalculateService(database);
            CalculateResult = new ArrayList { false, false, string.Empty, string.Empty, string.Empty, 0, 0 };
        }

        public void OnGet()
        {
            ViewData["hidden"] = "hidden";
            ViewData["visibility"] = "visible";
            ViewData["fixpayment"] = true;
            Emploees = _context.Emploees.AsNoTracking().ToList();
        }

        //This is the main post method in which the server receives data from the user.
        public async Task<IActionResult> OnPost(string firstname, string lastname, bool fixpayment, int? days, int? hours, int rate)
        {
            var resalt = Validation.InputValidation(firstname, lastname, days, hours, rate);
            if (resalt != "Ok")
            {
                ViewData["Message"] = resalt;
            }
            else
            {
                var emploee = new Emploees
                {
                    F_NAME = firstname,
                    L_NAME = lastname,
                    FIX_PAYMENT = fixpayment,
                    COUNT_DAYS = days,
                    COUNT_HOUR = hours,
                    RATE = rate
                };
                await _context.Emploees.AddAsync(emploee);
                await _context.SaveChangesAsync();
                return Redirect("/Index");
            }
            //This is the inverse filling of the page with data in case of incorrect entries.
            Emploees = _context.Emploees.AsNoTracking().ToList();
            ViewData["firstname"] = firstname;
            ViewData["lastname"] = lastname;
            ViewData["fixpayment"] = fixpayment;
            ViewData["days"] = days;
            ViewData["hours"] = hours;
            ViewData["rate"] = rate;
            ViewData["hidden"] = "visible";
            ViewData["visibility"] = "hidden";

            return Page();
        }

        //This method is called asynchronously to calculate wages.
        public IActionResult OnPostCalculate()
        {
            var emploeeId = Convert.ToInt32(Request.Query["emploeeId"]);
            var emploee = _context.Emploees.FirstOrDefault(p => p.ID == emploeeId);
            // This is the calculation for non-fixed employees.
            if (!emploee.FIX_PAYMENT)
            {
                var salaryamount = _calculate.Consider(emploee.F_NAME, emploee.L_NAME);
                CalculateResult[0] = emploee.FIX_PAYMENT;
                CalculateResult[2] = emploee.F_NAME;
                CalculateResult[3] = emploee.L_NAME;
                CalculateResult[5] = salaryamount;
                CalculateResult[6] = emploee.COUNT_HOUR;
                return new JsonResult(CalculateResult);
            }
            // This is the calculation for fixed employees.
            else
            {
                //This is the validation of data from calendars.
                var firstdate = Request.Query["firstdate"];
                var lastdate = Request.Query["lastdate"];
                var validationresult = Validation.DateValidation(firstdate, lastdate);
                CalculateResult[0] = true;
                CalculateResult[1] = validationresult[0];
                CalculateResult[4] = validationresult[1];
                if (!Convert.ToBoolean(validationresult[0])) return new JsonResult(CalculateResult);

                // This is the calculation for fixed employees.
                var FirstDate = Convert.ToDateTime(firstdate);
                var LastDate = Convert.ToDateTime(lastdate);
                var salaryamount = _calculate.ConsiderFix(emploee.F_NAME, emploee.L_NAME, FirstDate, LastDate);
                CalculateResult[0] = emploee.FIX_PAYMENT;
                CalculateResult[2] = emploee.F_NAME;
                CalculateResult[3] = emploee.L_NAME;
                CalculateResult[5] = salaryamount[0];
                CalculateResult[6] = salaryamount[1];
                return new JsonResult(CalculateResult);
            }
        }
    }
}


