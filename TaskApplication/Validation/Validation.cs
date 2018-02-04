using System;
using System.Collections;

namespace TaskApplication.Validations
{
    public static class Validation
    {
        //This is the main method for validating the user data entered.
        public static string InputValidation(string firstname, string lastname, int? days, int? hours, int rate)
        {
            try
            {
                if (!StringValid(firstname)) return "Пустая строка!";
                if (!StringValid(lastname)) return "Пустая строка!";
                var parsDays = Pars(days);
                var parsHours = Pars(hours);
                var parsRate = Pars(rate);
                if (rate <= 0) return "Введено число меньше или равно 0 или пустая строка!";
                if (!parsRate) return "Введено нечисловое выражение !";
                if (!parsDays) return "Введено нечисловое выражение !";
                if (!parsHours) return "Введено нечисловое выражение !";
                if (days == null && hours == null) return "Пустая строка!";
                if (days == null && hours != null && hours > 0) return "Ok";
                if (hours == null && days != null && days > 0) return "Ok";
                return "Неверный ввод!";
            }
            catch (Exception)
            {

                return "Неверный ввод!";
            }
        }
        
        //This is validation of entered dates.
        public static ArrayList DateValidation(string firstdate, string lastdate)
        {
            if (string.IsNullOrEmpty(firstdate) || string.IsNullOrEmpty(lastdate))
                return new ArrayList { false, "Не введена дата(ы)!" };
            var FirstDate = Convert.ToDateTime(firstdate);
            var LastDate = Convert.ToDateTime(lastdate);
            if (LastDate <= FirstDate)
                return new ArrayList { false, "Конечная дата меньше или равна начальной!" };

            return new ArrayList() { true, string.Empty };
        }
        public static bool StringValid(string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) return true;
            return false;
        }

        public static bool Pars(int value)
        {
            return Int32.TryParse(value.ToString(), out int res);
        }
        public static bool Pars(int? value)
        {
            if (value != null)
            {
                return Int32.TryParse(value.ToString(), out int res);
            }
            return true;
        }
    }
}
