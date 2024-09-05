namespace PokerDrill.Core.Data
{
    public record HandStrategyModel(
        // The name of the hand, like AA, or J9o.
        string Hand,

        // How often the hand is in the player's range.
        double Weight,

        // Each element represents the frequency for an action the user can take. Always sums to 1.
        double[] Strategy,

        // The colors to use for each action in the strategy.
        string[] StrategyColors

    );
}
