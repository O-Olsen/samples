namespace PokerDrill.UI.Controls.Converters
{
    using PokerDrill.Core.Data;
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class HandStrategyColorsToBrushConverter : IValueConverter
    {
        public static HandStrategyColorsToBrushConverter Instance { get; } = new HandStrategyColorsToBrushConverter();

        private HandStrategyColorsToBrushConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = (HandStrategyModel)value;

            var resultBrush = new LinearGradientBrush()
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(1, 0),

            };

            var foldColor = (Color)ColorConverter.ConvertFromString(model.StrategyColors[0]);
            var callColor = (Color)ColorConverter.ConvertFromString(model.StrategyColors[1]);
            var raiseColor = (Color)ColorConverter.ConvertFromString(model.StrategyColors[2]);

            resultBrush.GradientStops.Add(new GradientStop(foldColor, 0.0));
            resultBrush.GradientStops.Add(new GradientStop(foldColor, model.Strategy[0]));

            resultBrush.GradientStops.Add(new GradientStop(callColor, model.Strategy[0]));
            resultBrush.GradientStops.Add(new GradientStop(callColor, model.Strategy[1]));

            resultBrush.GradientStops.Add(new GradientStop(raiseColor, model.Strategy[1]));
            resultBrush.GradientStops.Add(new GradientStop(raiseColor, 1.0));
            return resultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}