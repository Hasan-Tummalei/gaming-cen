using System.Text.RegularExpressions;

namespace GamingCenter.Domain.ValueObjects.Membership
{
    public sealed record MemberShipTitle

    {
        public string Value { get; private init; }

        private MemberShipTitle() { }

        public MemberShipTitle(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name must not be null or empty");
            Value = name;

        }


  


    }
}
