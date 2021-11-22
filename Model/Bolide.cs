namespace Model
{
    public class Bolide : IEquipment
    {
        public Bolide(int performance, int quality, int speed, bool isBroken)
        {
            Performance = performance;
            Quality = quality;
            Speed = speed;
            IsBroken = isBroken;
        }

        public Bolide()
        {
        }

        public int Performance { get; set; }
        public int Quality { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

    }
}