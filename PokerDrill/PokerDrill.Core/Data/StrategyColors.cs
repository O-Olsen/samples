namespace PokerDrill.Core.Data
{
    using System.Windows.Media;

    public abstract record StrategyColors(string? Fold, string? Call, string? Raise)
    {
        private static ColorConverter _colorConverter { get; } = new ColorConverter();

        private record StrategyColorsImp(string? Fold, string? Call, string? Raise) : StrategyColors(Fold, Call, Raise);

        public static StrategyColors Create(string? Fold, string? Call, string? Raise)
        {
            ThrowOnInvalidInput(Fold, Call, Raise);
            return new StrategyColorsImp(Fold, Call, Raise);
        }

        private static void ThrowOnInvalidInput(string? fold, string? call, string? raise)
        {
            // TODO
        }
    }
}