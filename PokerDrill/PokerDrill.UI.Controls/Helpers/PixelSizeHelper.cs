namespace PokerDrill.UI.Controls.Helpers
{
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    internal class PixelSizeHelper
    {
        private static object _locker = new object();

        private static Rectangle _sampleVisual;

        private static float _pixelsPerDip;

        static PixelSizeHelper()
        {
        }

        public static void Init(Dispatcher uiThreadDispatcher)
        {
            uiThreadDispatcher.Invoke(
                () =>
                {
                    if (_sampleVisual == null)
                    {
                        _sampleVisual = new Rectangle();
                    }

                    _pixelsPerDip = (float)VisualTreeHelper.GetDpi(_sampleVisual).PixelsPerDip;
                },
                DispatcherPriority.Send);
        }

        public static float GetPixelPerDip()
        {
            return _pixelsPerDip;
        }

    }
}
