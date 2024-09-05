namespace PokerDrill.Core.Data.Helpers
{
    using System;

    /// Probably better be non-static and registered as singleton within dependency container.
    public static class HandStrategyHelper
    {
        private const int _rowsCount = 13;

        private const int _columnsCount = 13;

        private static readonly int _count = _rowsCount * _columnsCount;

        public static int RowsCount => _rowsCount;

        public static int ColumnsCount => _columnsCount;

        public static int Count => _count;

        private const string FoldColorString = "#FFcc9c8a";
        private const string CallColorString = "#FFc98b72";
        private const string RaiseColorString = "#FFbc7f68";

        private static readonly string[,] _names = new string[_rowsCount, _columnsCount]
        {
            { "AA", "AKs", "AQs", "AJs", "ATs", "A9s", "A8s", "A7s", "A6s", "A5s", "A4s", "A3s", "A2s" },
            { "AKo", "KK",  "KQs", "KJs", "KTs", "K9s", "K8s", "K7s", "K6s", "K5s", "K4s", "K3s", "K2s" },
            { "AQo", "KQo", "QQ",  "QJs", "QTs", "Q9s", "Q8s", "Q7s", "Q6s", "Q5s", "Q4s", "Q3s", "Q2s" },
            { "AJo", "KJo", "QJo", "JJ",  "JTs", "J9s", "J8s", "J7s", "J6s", "J5s", "J4s", "J3s", "J2s" },
            { "ATo", "KTo", "QTo", "JTo", "TT",  "T9s", "T8s", "T7s", "T6s", "T5s", "T4s", "T3s", "T2s" },
            { "A9o", "K9o", "Q9o", "J9o", "T9o", "99",  "98s", "97s", "96s", "95s", "94s", "93s", "92s" },
            { "A8o", "K8o", "Q8o", "J8o", "T8o", "98o", "88",  "87s", "86s", "85s", "84s", "83s", "82s" },
            { "A7o", "K7o", "Q7o", "J7o", "T7o", "97o", "87o", "77",  "76s", "75s", "74s", "73s", "72s" },
            { "A6o", "K6o", "Q6o", "J6o", "T6o", "96o", "86o", "76o", "66",  "65s", "64s", "63s", "62s" },
            { "A5o", "K5o", "Q5o", "J5o", "T5o", "95o", "85o", "75o", "65o", "55",  "54s", "53s", "52s" },
            { "A4o", "K4o", "Q4o", "J4o", "T4o", "94o", "84o", "74o", "64o", "54o", "44",  "43s", "42s" },
            { "A3o", "K3o", "Q3o", "J3o", "T3o", "93o", "83o", "73o", "63o", "53o", "43o", "33",  "32s" },
            { "A2o", "K2o", "Q2o", "J2o", "T2o", "92o", "82o", "72o", "62o", "52o", "42o", "32o", "22" }
        };

        public static string GetHandNameByRowAndColumn(int row, int column)
        {
            ThrowIfRowIsOutOfBounds(row);
            ThrowIfColumnIsOutOfRange(column);
            return _names[row, column];
        }

        public static string GetHandNameByIndex(int index)
        {
            ThrowIfIndexIsOutOfRange(index);
            var row = index / _rowsCount;
            var column = index % _rowsCount;
            return GetHandNameByRowAndColumn(row, column);
        }

        private static void ThrowIfIndexIsOutOfRange(int index)
        {
            if (index < 0 || index > _count)
            {
                throw new IndexOutOfRangeException($"The index was out of range. Expected value is '0' to '{_count}'");
            }
        }

        private static void ThrowIfColumnIsOutOfRange(int column)
        {
            if (column < 0 || column > _columnsCount)
            {
                throw new IndexOutOfRangeException($"The horizontal index was out of range. Expected value is '0' to '{_columnsCount}'");
            }
        }

        private static void ThrowIfRowIsOutOfBounds(int row)
        {
            if (row < 0 || row > _rowsCount)
            {
                throw new IndexOutOfRangeException($"The vertical index was out of range. Expected value is '0' to '{_rowsCount}'");
            }
        }

        public static int GetColumnsCount(this IEnumerable<HandStrategyModel> source)
        {
            return ColumnsCount;
        }

        public static int GetRowsCount(this IEnumerable<HandStrategyModel> source)
        {
            return RowsCount;
        }

        public static int GetCount(this IEnumerable<HandStrategyModel> source)
        {
            return Count;
        }

#if DEBUG
        public static HandStrategyModel GenerateDebugModel(int row, int column)
        {
            var strategy0 = Random.Shared.Next(2, 20) / 100.0;
            var strategy1 = Random.Shared.Next(20, 60) / 100.0;
            var strategy2 = 1.0 - strategy0 - strategy1;
            var weight = Random.Shared.NextDouble();
            var result = new HandStrategyModel(
                GetHandNameByRowAndColumn(row, column),
                weight,
                [strategy0, strategy1, strategy2],
                [FoldColorString, CallColorString, RaiseColorString]);

            return result;
        }
    }
#endif
}
