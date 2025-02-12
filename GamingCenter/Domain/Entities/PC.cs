using GamingCenter.Domain.ValueObjects.PC;

namespace GamingCenter.Domain.Entities
{
    public class PC
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Cost Cost { get; private set; }
        public PerformanceRate Performance { get; private set; }

        public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();


        private PC() { } 
        public PC (Cost cost, PerformanceRate performance)
        {
            Cost = cost;
            Performance = performance;
        }


        public void UpdatePc(Cost cost, PerformanceRate performance)
        {
            Cost = cost;
            Performance = performance;
        }
    }
}
