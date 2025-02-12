namespace GamingCenter.Domain.ValueObjects.User
{
    public sealed record DoB
    {
        public DateTime Value { get; private init; }


        private DoB() { }
        private DoB(DateTime dob) 
        {
            if (dob.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Date of Birth must be in UTC.");
            }
            Value = dob;
        }


        public static DoB Create(DateTime dob)
        {
            if (dob > DateTime.Now) throw new ArgumentException("Date of birth must not be in the future");
            if (dob < DateTime.Now.AddYears(-150)) throw new ArgumentException("Age must be applicable");

            return new DoB(dob.ToUniversalTime());
        }
    }
}
