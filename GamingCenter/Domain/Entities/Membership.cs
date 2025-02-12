using GamingCenter.Domain.ValueObjects.Membership;

namespace GamingCenter.Domain.Entities
{
    public class Membership
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public MemberShipTitle MembershipName { get; private set; }
        public DiscountPercentage DiscountPercentage { get; private set; }
        public HoursThreshold HoursThreshold { get; private set; }
        public Guid UserId { get; private set; }
        public ICollection<User> Users { get; private set; }

        private Membership() { }

        public Membership(MemberShipTitle membership, DiscountPercentage discountPercentage, HoursThreshold hoursThreshold) 
        { 
            MembershipName = membership;
            DiscountPercentage = discountPercentage;
            HoursThreshold = hoursThreshold;
        
        }

        public void UpdateMembership(MemberShipTitle? memberShipTitle = null, DiscountPercentage? discountPercentage = null, HoursThreshold? hourThreshold = null)
        {
            if (memberShipTitle != null)
                MembershipName = memberShipTitle;

            if (discountPercentage != null)
                DiscountPercentage = discountPercentage;

            if (hourThreshold != null)
                HoursThreshold = hourThreshold;
        }
    }


}

