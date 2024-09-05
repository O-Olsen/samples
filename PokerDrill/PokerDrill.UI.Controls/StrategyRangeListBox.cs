namespace PokerDrill.UI.Controls
{
    using PokerDrill.UI.Controls.Helpers;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class StrategyRangeListBox : ListBox
    {
        private Stopwatch _ctorToRenderWatch;

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(StrategyRangeListBox), new PropertyMetadata());

        public StrategyRangeListBox()
        {
            _ctorToRenderWatch = Stopwatch.StartNew();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (e.AddedItems.Count > 0)
            {
                Command?.Execute(e.AddedItems[0]);
                SelectedItem = null;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
#if DEBUG
            Trace.WriteLine($"{nameof(StrategyRangeListBox)} ctor to render time updated:  {_ctorToRenderWatch.GetMiliseconds()}ms.");
#endif
        }
    }
}