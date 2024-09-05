namespace PokerDrill.UI.Controls
{
    using PokerDrill.Core.Data;
    using System.Windows;
    using System.Windows.Media;

    public class StrategyPresentationHelper : FrameworkElement
    {
        private HandStrategyModel _model;

        private static Dictionary<string, SolidColorBrush> _brushesCache = new Dictionary<string, SolidColorBrush>();

        private static readonly SolidColorBrush _weightBarBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8b8b8b"));

        #region DPs
        public double WeightBarHeight
        {
            get => (double)GetValue(WeightHeightProperty);
            private set => SetValue(WeightHeightProperty, value);
        }

        public static readonly DependencyProperty WeightHeightProperty =
            DependencyProperty.Register("WeightBarHeight", typeof(double), typeof(StrategyPresentationHelper), new PropertyMetadata());

        public double FoldBarWidth
        {
            get => (double)GetValue(FoldBarWidthProperty);
            private set => SetValue(FoldBarWidthProperty, value);
        }

        public static readonly DependencyProperty FoldBarWidthProperty =
            DependencyProperty.Register("FoldBarWidth", typeof(double), typeof(StrategyPresentationHelper), new PropertyMetadata());

        public SolidColorBrush FoldBarBrush
        {
            get => (SolidColorBrush)GetValue(FoldBarBrushProperty);
            set => SetValue(FoldBarBrushProperty, value);
        }

        public static readonly DependencyProperty FoldBarBrushProperty =
            DependencyProperty.Register("FoldBarBrush", typeof(SolidColorBrush), typeof(StrategyPresentationHelper), new PropertyMetadata());

        public double CallBarWidth
        {
            get => (double)GetValue(CallBarWidthProperty);
            private set => SetValue(CallBarWidthProperty, value);
        }

        public static readonly DependencyProperty CallBarWidthProperty =
            DependencyProperty.Register("CallBarWidth", typeof(double), typeof(StrategyPresentationHelper), new PropertyMetadata());

        public SolidColorBrush CallBarBrush
        {
            get => (SolidColorBrush)GetValue(CallBarBrushProperty);
            private set => SetValue(CallBarBrushProperty, value);
        }

        public static readonly DependencyProperty CallBarBrushProperty =
            DependencyProperty.Register("CallBarBrush", typeof(SolidColorBrush), typeof(StrategyPresentationHelper), new PropertyMetadata());

        public double RaiseBarWidth
        {
            get => (double)GetValue(RaiseBarWidthProperty);
            private set => SetValue(RaiseBarWidthProperty, value);
        }

        public static readonly DependencyProperty RaiseBarWidthProperty =
            DependencyProperty.Register("RaiseBarWidth", typeof(double), typeof(StrategyPresentationHelper), new PropertyMetadata());

        public SolidColorBrush RaiseBarBrush
        {
            get => (SolidColorBrush)GetValue(RaiseBarBrushProperty);
            private set => SetValue(RaiseBarBrushProperty, value);
        }

        public static readonly DependencyProperty RaiseBarBrushProperty =
            DependencyProperty.Register("RaiseBarBrush", typeof(SolidColorBrush), typeof(StrategyPresentationHelper), new PropertyMetadata());

        #endregion

        #region Static methods
        public static bool IsBold(HandStrategyModel model) => model.Hand.Length == 2;

        public static SolidColorBrush GetWeightBrush(HandStrategyModel model) => _weightBarBrush;

        public static SolidColorBrush GetFoldBrush(HandStrategyModel model)
        {
            if (!_brushesCache.TryGetValue(model.StrategyColors[0], out var foldBrush))
            {
                foldBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.StrategyColors[0]));
                _brushesCache.Add(model.StrategyColors[0], foldBrush);
            }

            return foldBrush;
        }

        public static SolidColorBrush GetCallbrush(HandStrategyModel model)
        {
            if (!_brushesCache.TryGetValue(model.StrategyColors[1], out var callBrush))
            {
                callBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.StrategyColors[1]));
                _brushesCache.Add(model.StrategyColors[1], callBrush);
            }

            return callBrush;
        }

        public static SolidColorBrush GetRaiseBrush(HandStrategyModel model)
        {
            if (!_brushesCache.TryGetValue(model.StrategyColors[2], out var raiseBrush))
            {
                raiseBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.StrategyColors[2]));
                _brushesCache.Add(model.StrategyColors[2], raiseBrush);
            }

            return raiseBrush;
        }
        #endregion

        #region Overrides
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DataContextProperty && e.NewValue is HandStrategyModel model)
            {
                OnModelAcquired(model);
            }

            if (e.Property == ActualWidthProperty || e.Property == ActualHeightProperty)
            {
                UpdatePresentationData();
            }
        }
        #endregion

        #region Private methods
        private void OnModelAcquired(HandStrategyModel model)
        {
            _model = model;
            FoldBarBrush = GetFoldBrush(_model);
            CallBarBrush = GetCallbrush(_model);
            RaiseBarBrush = GetRaiseBrush(_model);
            UpdatePresentationData();
        }

        private void UpdatePresentationData()
        {
            if (_model == null || ActualHeight == 0.0 || ActualWidth == 0.0)
            {
                return;
            }

            FoldBarWidth = ActualWidth * _model.Strategy[0];
            CallBarWidth = ActualWidth * _model.Strategy[1];
            RaiseBarWidth = ActualWidth * _model.Strategy[2];
            WeightBarHeight = ActualHeight * _model.Weight;
        }
        #endregion
    }
}
