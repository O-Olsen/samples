namespace PokerDrill.UI.Controls
{
    using PokerDrill.Core.Data;
    using PokerDrill.Core.Data.Helpers;
    using PokerDrill.UI.Controls.Helpers;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public sealed class StrategyRangePresenter : ContentControl
    {
        #region Static
        private static readonly DiuHelper _diuHelper = new();
        #endregion

        #region Fields
        private Typeface _regularTypeface;

        private Typeface _boldTypeface;

        private readonly FormattedText[] _cellTextContents = new FormattedText[HandStrategyHelper.Count];

        private double _localVerticalOffset;

        private DrawingBrush _gridBrush;

        private GeometryDrawing _gridDrawing;

        private LineGeometry _leftLine;

        private LineGeometry _topLine;

        private Pen _gridPen;

        private double _cellWidth;

        private double _cellHeight;

        private bool _rerenderRequired;
        #endregion

        #region Properties
        private bool CanRender => ItemsSource != null && _cellWidth != 0.0 && _cellHeight != 0.0;
        #endregion

        #region DPs
        /// <summary>
        /// Gets or sets the items source
        /// </summary>
        public IEnumerable<HandStrategyModel> ItemsSource
        {
            get { return (IEnumerable<HandStrategyModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<HandStrategyModel>), typeof(StrategyRangePresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets the grid lines brush
        /// </summary>
        public Brush GridLinesBrush
        {
            get => (Brush)GetValue(GridLinesBrushProperty);
            set => SetValue(GridLinesBrushProperty, value);
        }

        public static readonly DependencyProperty GridLinesBrushProperty =
            DependencyProperty.Register("GridLinesBrush", typeof(Brush), typeof(StrategyRangePresenter), new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets the command that should be executed upon item click.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(StrategyRangePresenter), new PropertyMetadata());
        #endregion

        #region Debug
        private Stopwatch _ctorToRenderWatch;

        #endregion

        #region Ctor
        public StrategyRangePresenter()
        {
#if DEBUG
            _ctorToRenderWatch = Stopwatch.StartNew();
#endif
            _regularTypeface = new Typeface(FontFamily, FontStyle, FontWeights.Normal, FontStretch);
            _boldTypeface = new Typeface(FontFamily, FontStyle, FontWeights.Bold, FontStretch);
            // Grid rendering setup
            _leftLine = new LineGeometry();
            _topLine = new LineGeometry();
            _gridPen = new Pen(GridLinesBrush, 1.0);
            _gridDrawing = new GeometryDrawing() { Pen = _gridPen };
            var gridGeometryGroup = new GeometryGroup();
            gridGeometryGroup.Children.Add(_leftLine);
            gridGeometryGroup.Children.Add(_topLine);
            _gridDrawing.Geometry = gridGeometryGroup;
            _gridBrush = new DrawingBrush
            {
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
                Drawing = _gridDrawing
            };

            SizeChanged += StrategyRangePresenter_SizeChanged;
            Loaded += StrategyRangePresenter_Loaded;
        }
        #endregion

        #region Overrides
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Equals(ItemsSourceProperty))
            {
                OnItemsSourceChanged((IEnumerable<HandStrategyModel>)e.OldValue, (IEnumerable<HandStrategyModel>)e.NewValue);
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            var clickPoint = e.GetPosition(this);
            var columnIndex = Math.Min(Math.Max(0, (int)(clickPoint.X / _cellWidth)), HandStrategyHelper.ColumnsCount - 1);
            var rowIndex = Math.Min(Math.Max(0, (int)(clickPoint.Y / _cellHeight)), HandStrategyHelper.RowsCount - 1);

            // How to get the hand name if required.
            //var text = HandStrategyHelper.GetHandNameByRowAndColumn(rowIndex, columnIndex);

            // Executing the command, if available
            Command?.Execute(ItemsSource.ElementAt((rowIndex * HandStrategyHelper.ColumnsCount) + columnIndex));
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles the Loaded event, and subscribes to owner window's StateChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StrategyRangePresenter_Loaded(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.StateChanged += ParentWindow_StateChanged;
            }
        }

        /// <summary>
        /// Handles the main window's StateChanged event, and sets the flag that fixes the incorrect rendering after maximizing or restoring the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindow_StateChanged(object? sender, EventArgs e)
        {
            _rerenderRequired = true;
        }

        /// <summary>
        /// Handles the size change of the control, and adjusts the grid brush's inner dimensions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StrategyRangePresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_cellWidth == 0)
            {
                _rerenderRequired = true;
            }

            _cellWidth = e.NewSize.Width / HandStrategyHelper.ColumnsCount;
            _cellHeight = e.NewSize.Height / HandStrategyHelper.RowsCount;
            _gridBrush.Viewport = new Rect(0, 0, _cellWidth, _cellHeight);
            _leftLine.EndPoint = new Point(0.0, _cellHeight);
            _topLine.EndPoint = new Point(_cellWidth, 0.0);

            if (_rerenderRequired)
            {
                _rerenderRequired = false;
                InvalidateVisual();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Handles the change of the items source
        /// </summary>
        /// <param name="former"></param>
        /// <param name="actual"></param>
        private void OnItemsSourceChanged(IEnumerable<HandStrategyModel> former, IEnumerable<HandStrategyModel> actual)
        {
            if (former != null)
            {
                Cleanup();
            }

            if (actual != null)
            {
                LoadCurrentSource();
            }
        }

        /// <summary>
        /// Prepares for rendering of the current items source
        /// </summary>
        private void LoadCurrentSource()
        {
            var index = 0;
            foreach (var cell in ItemsSource)
            {
                var ft = new FormattedText(
                    cell.Hand,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    StrategyPresentationHelper.IsBold(cell) ? _boldTypeface : _regularTypeface,
                    FontSize,
                    Foreground,
                    PixelSizeHelper.GetPixelPerDip());
                _cellTextContents[index] = ft;
                index++;
            }

            _localVerticalOffset = _cellTextContents[0].Baseline / 2.0;
            InvalidateVisual();
        }

        /// <summary>
        /// Performs a cleanup
        /// </summary>
        private void Cleanup()
        {
            // TODO Finish the cleanup process
            Array.Clear(_cellTextContents, 0, _cellTextContents.Length);
        }
        #endregion

        #region Rendering
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (!CanRender)
            {
                return;
            }

            DrawStats(drawingContext);
            DrawGrid(drawingContext);
            DrawItems(drawingContext);
#if DEBUG
            Trace.WriteLine($"{nameof(StrategyRangePresenter)} ctor to render time updated:  {_ctorToRenderWatch.GetMiliseconds()}ms.");

#endif
        }

        /// <summary>
        /// Draws the Weight, Fold, Call and Raise stats bars.
        /// </summary>
        /// <param name="context"></param>
        private void DrawStats(DrawingContext context)
        {
            var x = 0.0;
            var y = 0.0;
            var columnIndex = 0;
            foreach (var model in ItemsSource)
            {
                // Drawing the Fold bar
                var foldBarWidth = _cellWidth * model.Strategy[0];
                var foldBarX = x;
                context.DrawRectangle(StrategyPresentationHelper.GetFoldBrush(model),
                    null,
                    new Rect(foldBarX, y, foldBarWidth, _cellHeight));

                // Drawing the Call bar
                var callBarWidth = _cellWidth * model.Strategy[1];
                var callBarX = x + foldBarWidth;
                context.DrawRectangle(StrategyPresentationHelper.GetCallbrush(model),
                    null,
                    new Rect(callBarX, y, callBarWidth, _cellHeight));

                // Drawing the Raise bar
                var raiseBarWidth = _cellWidth * model.Strategy[2];
                var raiseBarX = callBarX + callBarWidth;
                context.DrawRectangle(StrategyPresentationHelper.GetRaiseBrush(model),
                    null,
                    new Rect(raiseBarX, y, raiseBarWidth, _cellHeight));

                // Drawing the Weight bar
                context.DrawRectangle(StrategyPresentationHelper.GetWeightBrush(model),
                    null,
                    new Rect(x, y, _cellWidth, _cellHeight * model.Weight));
                columnIndex++;
                if (columnIndex < HandStrategyHelper.ColumnsCount)
                {
                    x += _cellWidth;
                }
                else
                {
                    columnIndex = 0;
                    x = 0.0;
                    y += _cellHeight;
                }
            }
        }

        /// <summary>
        /// Draws the grid lines.
        /// </summary>
        /// <param name="context"></param>
        private void DrawGrid(DrawingContext context)
        {
            context.DrawRectangle(_gridBrush, null, new Rect(0, 0, ActualWidth, ActualHeight));
            context.DrawLine(_gridPen, new Point(ActualWidth, 0), new Point(ActualWidth, ActualHeight));
            context.DrawLine(_gridPen, new Point(0, ActualHeight), new Point(ActualWidth, ActualHeight));
        }

        /// <summary>
        /// Draws the hands' headers
        /// </summary>
        /// <param name="context"></param>
        private void DrawItems(DrawingContext context)
        {
            var x = _cellWidth / 2.0;
            var y = _cellHeight / 2.0;
            var textIndex = 0;
            for (var rowIndex = 0; rowIndex < HandStrategyHelper.RowsCount; rowIndex++)
            {
                for (var columnindex = 0; columnindex < HandStrategyHelper.ColumnsCount; columnindex++)
                {
                    var text = _cellTextContents[textIndex];
                    context.DrawText(text, new Point(x - (text.Width / 2.0), y - _localVerticalOffset));
                    x += _cellWidth;
                    textIndex++;
                }

                y += _cellHeight;
                x = _cellWidth / 2.0;
            }
        }
        #endregion

        #region Helper methods
        private static double Snap(double length) => _diuHelper.SnapDimensionToCurrentDpi(length);

        private static Rect Snap(Rect rect) => _diuHelper.SnapRectToCurrentDpi(rect);
        #endregion
    }
}
