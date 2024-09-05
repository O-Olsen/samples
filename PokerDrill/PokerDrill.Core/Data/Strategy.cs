namespace PokerDrill.Core.Data
{
    using System;

    public abstract record Strategy(double Fold, double Call, double Raise)
    {
        public static Strategy Create(double fold, double call, double raise)
        {
            ThrowOnInvalidInput(fold, call, raise);
            return new StrategyImp(fold, call, raise);
        }

        private record StrategyImp(double Fold, double Call, double Raise) : Strategy(Fold, Call, Raise);

        private static void ThrowOnInvalidInput(double fold, double call, double raise)
        {
            if (fold < 0 || call < 0 || raise < 0
                || fold + call + raise != 1.0)
            {
                throw new ArgumentOutOfRangeException($"The parameters must be positive, and the '{nameof(fold)}+{nameof(call)}+{nameof(raise)}' should be equal to 1.0.");
            }
        }
    }
}
