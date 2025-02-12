using System.Text.RegularExpressions;

namespace GamingCenter.Domain.ValueObjects.User
{
    public class Email
    {
        public string Value {  get; private init; }
        private static readonly string _emailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";

        /*
            Example Matches:
            example@example.com
            user.name+tag+sorting@example.com
            user@sub.example.com
            user@123.123.123.123 (valid for some systems)
            user@[IPv6:2001:db8::1] (valid for some systems)      
        */

        private Email() { }
        private Email(string email)
        {
            Value = email;
        }


        public static Email Create(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("Email can't be null or empty");
            if (!Regex.IsMatch(email, _emailPattern)) throw new ArgumentException("Email is Invalid");

            return new Email(email);
        }
    }
}
