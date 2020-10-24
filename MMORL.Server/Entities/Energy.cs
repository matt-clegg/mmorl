namespace MMORL.Server.Entities
{
    public class Energy
    {
        public const int ActionCost = 12;

        public const int MinSpeed = 0;
        public const int NormalSpeed = 3;
        public const int MaxSpeed = 5;

        private static readonly int[] _energyGains = { 2, 3, 4, 6, 9, 12 };

        public int CurrentEnergy { get; private set; }
        public bool CanTakeTurn => CurrentEnergy >= ActionCost;

        public void Spend()
        {
            if (CanTakeTurn)
            {
                CurrentEnergy %= ActionCost;
            }
        }

        public bool Gain(int speed)
        {
            if (speed < MinSpeed)
            {
                speed = MinSpeed;
            }

            if (speed >= MaxSpeed)
            {
                speed = MaxSpeed;
            }

            CurrentEnergy += _energyGains[speed];
            return CanTakeTurn;
        }

        public void Reset() => CurrentEnergy = 0;
    }
}
