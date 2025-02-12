using System.Text.RegularExpressions;

namespace GamingCenter.Domain.ValueObjects.User
{
    public sealed record Name
    {
        public string Value { get; private init; }

        /*
            full name must only contain alphabetic characters (letters) and exactly one space between the two parts
            (first name and last name), and each part must be at least 2 characters long

                Examples:
                John Doe
                María García
                JeanLuc Picard
                Björk Guðmundsdóttir
        */
        private static readonly string namePattern = "^[A-Za-zÀ-ÖØ-öø-ÿ]{2,} [A-Za-zÀ-ÖØ-öø-ÿ]{2,}$";


        private Name() { }
        private  Name(string name ) {

            Value = name;

        }


        public static Name Create(string name) 
        { 

            if(string.IsNullOrEmpty(name))  throw new ArgumentNullException("Name must not be null or empty");
            if (!Regex.IsMatch(name, namePattern)) throw new ArgumentException("name must be of two parts, first name, last name, each of at least two characters, and shouldn't be an email");
            return new Name( name );
        }


    }
}
