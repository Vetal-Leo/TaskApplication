using System;


namespace TaskApplication.Validations
{
    public static class Validation
    {

        public static bool StringValid(string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) return true;
            return false;
        }

        public static bool Pars(int value)
        {
            int res;
            return Int32.TryParse(value.ToString(), out res);
        }
        public static bool Pars(int? value)
        {
            if (value != null)
            {
                int res;
                return Int32.TryParse(value.ToString(), out res);
            }
            return true;
        }

        public static string Valid(string firstname, string lastname, int? days, int? hours, int rate)
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

    }
}
