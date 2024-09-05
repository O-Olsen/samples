namespace PokerDrillDemo.ViewModels
{
    using PokerDrill.Core.Data;
    using PokerDrill.Core.Data.Helpers;
    using Prism.Commands;
    using Prism.Mvvm;
    using System.Windows;

    public sealed class MainViewModel : BindableBase
    {
        private IEnumerable<HandStrategyModel> _handStrategies;

        public IEnumerable<HandStrategyModel> HandStrategies
        {
            get { return _handStrategies; }
            set { SetProperty(ref _handStrategies, value); }
        }
        private DelegateCommand<HandStrategyModel> _strategyClickCommand;

        public DelegateCommand<HandStrategyModel> StrategyClickCommand => _strategyClickCommand ?? (_strategyClickCommand = new DelegateCommand<HandStrategyModel>(StrategyClickCommand_Execute));

        public MainViewModel()
        {
            var strategiesList = new List<HandStrategyModel>(HandStrategyHelper.Count);
#if DEBUG
            for (var row = 0; row < HandStrategyHelper.RowsCount; row++)
            {
                for (var column = 0; column < HandStrategyHelper.ColumnsCount; column++)
                {
                    strategiesList.Add(HandStrategyHelper.GenerateDebugModel(row, column));
                }
            }
#endif
            _handStrategies = strategiesList;
        }

        private void StrategyClickCommand_Execute(HandStrategyModel parameter)
        {
            MessageBox.Show($"Clicked hand is '{parameter.Hand}'.");
        }
    }
}
