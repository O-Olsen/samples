namespace PokerDrill.UI.Controls.Helpers
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A helper class for Dpi logic, because Microsoft hides this with the internal flag.
    /// </summary>
    public class DiuHelper
    {
        private readonly PropertyInfo? _dpiProperty;

        private Matrix _transformToDevice;

        private Matrix _transformToLogical;

        public double DpiX { get; private set; }

        public double DpiY { get; private set; }

        public DiuHelper()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            _dpiProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            if (dpiXProperty != null && _dpiProperty != null)
            {
                var pixelsPerInchX = (int)dpiXProperty.GetValue(null, null); // SystemParameters.DpiX;
                DpiX = pixelsPerInchX;
                var pixelsPerInchY = (int)_dpiProperty.GetValue(null, null); // SystemParameters.Dpi;
                DpiY = pixelsPerInchY;
                _transformToLogical = Matrix.Identity;
                _transformToLogical.Scale(96d / pixelsPerInchX, 96d / pixelsPerInchY);
                _transformToDevice = Matrix.Identity;
                _transformToDevice.Scale(pixelsPerInchX / 96d, pixelsPerInchY / 96d);
            }
        }

        /// <summary>
        /// Convert a point in device independent pixels (1/96") to a point in the system coordinates.
        /// </summary>
        /// <param name="logicalPoint">A point in the logical coordinate system.</param>
        /// <returns>Returns the point converted to the system's coordinates.</returns>
        public Point LogicalPixelsToDevice(Point logicalPoint)
        {
            return _transformToDevice.Transform(logicalPoint);
        }

        /// <summary>
        /// Convert a point in system coordinates to a point in device independent pixels (1/96").
        /// </summary>
        /// <param name="devicePoint">A point in the physical coordinate system.</param>
        /// <returns>Returns the point converted to the device independent coordinate system.</returns>
        public Point DevicePixelsToLogical(Point devicePoint)
        {
            return _transformToLogical.Transform(devicePoint);
        }

        public Rect LogicalRectToDevice(Rect logicalRectangle)
        {
            var topLeft = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top));
            var bottomRight = LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom));
            return new Rect(topLeft, bottomRight);
        }

        public Rect DeviceRectToLogical(Rect deviceRectangle)
        {
            var topLeft = DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top));
            var bottomRight = DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom));

            return new Rect(topLeft, bottomRight);
        }

        public Size LogicalSizeToDevice(Size logicalSize)
        {
            var pt = LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height));
            return new Size { Width = pt.X, Height = pt.Y };
        }

        public Size DeviceSizeToLogical(Size deviceSize)
        {
            var pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height));
            return new Size(pt.X, pt.Y);
        }

        public Thickness LogicalThicknessToDevice(Thickness logicalThickness)
        {
            var topLeft = LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top));
            var bottomRight = LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom));
            return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }

        public double SnapDimensionToCurrentDpi(double length)
        {
            if (length <= 0.0)
            {
                return 0.0;
            }

            var dpi = (int)(_dpiProperty?.GetValue(null, null) ?? 96);
            var pixelLength = 96.0 / dpi;
            var actualLength = length + pixelLength / 2.0;
            var multiplier = actualLength * 1000.0 / (pixelLength * 1000.0);
            actualLength = (int)multiplier * pixelLength;
            return actualLength;
        }

        public Thickness SnapThicknessToCurrentDpi(Thickness thickness)
        {
            return new Thickness(
                SnapDimensionToCurrentDpi(thickness.Left),
                SnapDimensionToCurrentDpi(thickness.Top),
                SnapDimensionToCurrentDpi(thickness.Right),
                SnapDimensionToCurrentDpi(thickness.Bottom));
        }

        public Rect SnapRectToCurrentDpi(Rect rect)
        {
            return new Rect(
                SnapDimensionToCurrentDpi(rect.X),
                SnapDimensionToCurrentDpi(rect.Y),
                SnapDimensionToCurrentDpi(rect.Width),
                SnapDimensionToCurrentDpi(rect.Height));
        }
    }
}