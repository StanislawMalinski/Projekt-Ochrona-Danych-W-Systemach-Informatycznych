

namespace projekt.Serivces
{
    public class Validator
    {
     
        public static bool validEmail(string email){ 
            var validChars = new HashSet<char> {'@', '.', '_', '-'};
            foreach (var c in email)
                if (!char.IsLetterOrDigit(c) && !validChars.Contains(c))
                    return false;
            return true;
        }

        public static  bool validCode(string code)
        {
            bool valid = true;
            foreach (var c in code)
                if (!char.IsDigit(c))
                    valid = false;
            valid = valid && code.Length == 6;
            return valid;
        }

        public static  bool validName(string name)
        {
            bool valid = true;
            foreach (var c in name)
                if (!char.IsLetter(c) || c == ' ' || c == '-')
                    valid = false;
            return valid;
        }

        public static  bool validNumber(string number)
        {
            bool valid = true;
            foreach (var c in number)
                if (!char.IsDigit(c) || c == '.' || c == ',')
                    valid = false;
            return valid;
        }

        public static bool validText(string text)
        {
            bool valid = true;
            foreach (var c in text)
                if (char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '.' || c == ',')
                    valid = false;
            return valid;
        }
    }
}