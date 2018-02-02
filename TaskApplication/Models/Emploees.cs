
using Microsoft.EntityFrameworkCore;


namespace TaskApplication.Models
{

    public class Emploees
    {
        public int ID { get; set; }
        public string F_NAME { get; set; }
        public string L_NAME { get; set; }
        public bool FIX_PAYMENT { get; set; }
        public int? COUNT_DAYS { get; set; }
        public int? COUNT_HOUR { get; set; }
        public int RATE { get; set; }
    }


    public class ApplicationContext : DbContext
    {
        public DbSet<Emploees> Emploees { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }

}
