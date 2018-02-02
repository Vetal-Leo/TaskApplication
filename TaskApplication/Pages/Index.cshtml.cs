using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public IndexModel(ApplicationContext database)
        {
            _context = database;
            _calculate = new CalculateService(database);
        }

        public void OnGet()
        {
            ViewData["hidden"] = "hidden";
            ViewData["visibility"] = "visible";
            Emploees = _context.Emploees.AsNoTracking().ToList();
        }

        public async Task<IActionResult> OnPost(string firstname, string lastname, bool fixpayment, int? days, int? hours, int rate)
        {
            var resalt = Validation.Valid(firstname, lastname, days, hours, rate);
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


        public IActionResult OnPostCalculate()
        {
            var emploee = Request.Query["emploeeId"];
            var emploeeId = JsonConvert.DeserializeObject<int>(emploee);
            var firstname = _context.Emploees.FirstOrDefault(p => p.ID == emploeeId).F_NAME;
            var lastname = _context.Emploees.FirstOrDefault(p => p.ID == emploeeId).L_NAME;
            var salaryamount = _calculate.Consider(firstname, lastname).ToString();
            var result = new List<string>() { firstname, lastname, salaryamount };
            Emploees = _context.Emploees.AsNoTracking().ToList();

            return new JsonResult(result);
        }
    }
}


