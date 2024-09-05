namespace PokerDrill.UI.Controls.Helpers
{
    using System.Diagnostics;

    public static class General
    {
        public static double GetMiliseconds(this Stopwatch stopwatch)
        {
            return (double)stopwatch.ElapsedTicks / (double)Stopwatch.Frequency * 1000.0;
        }
    }
}
