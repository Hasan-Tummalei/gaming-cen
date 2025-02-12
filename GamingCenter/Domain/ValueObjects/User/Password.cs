namespace GamingCenter.Domain.ValueObjects.User
{
    public sealed record Password
    {
        public string Value { get; private init; }

        private Password() {}
        private Password(string password)
        {
            Value = password;
        }
        public static Password Create(string BeHashedPassword)
        {

            if (string.IsNullOrEmpty(BeHashedPassword)) throw new ArgumentNullException("Password must not be emtpy");
            return new Password(BeHashedPassword);
        }


    }
}
