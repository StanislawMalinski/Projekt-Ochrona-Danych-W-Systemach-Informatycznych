

namespace projekt.Services
{
    public class Validator
    {
        private static IConfiguration? _configuration;

        public Validator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static bool validEmail(string email){ 
            var validChars = new HashSet<char> {'@', '.', '_', '-'};
            foreach (var c in email)
                if (!char.IsLetterOrDigit(c) && !validChars.Contains(c))
                    return false;
            return email.Contains('@');
        }

        public static bool validCode(string code)
        {
            bool valid = true;
            foreach (var c in code)
                if (!char.IsDigit(c))
                    valid = false;
            var lenCode = _configuration?.GetValue<int>("BankService:VerificationCodeLength");
            valid = valid && code.Length == ((lenCode != null) ? lenCode : 6);
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