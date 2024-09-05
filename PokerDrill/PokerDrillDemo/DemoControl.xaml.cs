namespace PokerDrillDemo
{
    using Prism.Commands;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DemoControl.xaml
    /// </summary>
    public partial class DemoControl : UserControl
    {
        public DataTemplate ListBoxTemplate
        {
            get => (DataTemplate)GetValue(ListBoxTemplateProperty);
            set => SetValue(ListBoxTemplateProperty, value);
        }

        public static readonly DependencyProperty ListBoxTemplateProperty =
            DependencyProperty.Register("ListBoxTemplate", typeof(DataTemplate), typeof(DemoControl), new PropertyMetadata());

        public DataTemplate RangePresenterTemplate
        {
            get => (DataTemplate)GetValue(RangePresenterTemplateProperty);
            set => SetValue(RangePresenterTemplateProperty, value);
        }

        public static readonly DependencyProperty RangePresenterTemplateProperty =
            DependencyProperty.Register("RangePresenterTemplate", typeof(DataTemplate), typeof(DemoControl), new PropertyMetadata());

        public DelegateCommand<DemoControlType?> SwitchToDemoControl
        {
            get => (DelegateCommand<DemoControlType?>)GetValue(SwitchToDemoControlProperty);
            private set => SetValue(SwitchToDemoControlProperty, value);
        }

        public static readonly DependencyProperty SwitchToDemoControlProperty =
            DependencyProperty.Register("SwitchToDemoControl", typeof(DelegateCommand<DemoControlType?>), typeof(DemoControl), new PropertyMetadata());

        public DemoControl()
        {
            InitializeComponent();
            SwitchToDemoControl = new DelegateCommand<DemoControlType?>(SwitchToDemoControl_Execute);
        }

        private void SwitchToDemoControl_Execute(DemoControlType? type)
        {
            contentControl.ContentTemplate = type switch
            {
                DemoControlType.ListBox => ListBoxTemplate,
                DemoControlType.RangePresenter => RangePresenterTemplate,
                _ => throw new NotImplementedException(),
            };
        }
    }

    public enum DemoControlType
    {
        ListBox,
        RangePresenter,
    }
}
