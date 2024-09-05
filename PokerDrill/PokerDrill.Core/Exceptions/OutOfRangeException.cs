namespace PokerDrill.Core.Exceptions
{
    using System;

    public class OutOfRangeException<TValue> : Exception
    {
        public static void Throw(string parameterName, TValue value, TValue from, TValue to, string additionalInfo = "")
        {
            throw new OutOfRangeException<TValue>(parameterName, value, from, to, additionalInfo);
        }

        public OutOfRangeException(string parameterName, TValue value, TValue from, TValue to, string additionalInfo)
            : base($"The '{parameterName}' was out of expected range of '{from}..{to}'. Additionally, {additionalInfo}.")
        {
        }
    }
}
