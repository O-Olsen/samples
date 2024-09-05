namespace PokerDrill.Core.Data
{
    using PokerDrill.Core.Exceptions;

    public abstract record Weight(double Value)
    {
        private record WeightImp(double Value) : Weight(Value);

        public static Weight Create(double value)
        {
            ThrowOnInvalidInput(value);
            return new WeightImp(value);
        }

        private static void ThrowOnInvalidInput(double value)
        {
            if (value < 0.0 && value > 1.0)
            {
                OutOfRangeException<double>.Throw(nameof(value), value, 0.0, 1.0);
            }
        }
    }
}
