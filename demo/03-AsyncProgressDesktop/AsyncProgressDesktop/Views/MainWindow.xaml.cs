using AsyncProgressDesktop.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace AsyncProgressDesktop.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            this.WhenActivated(disposableRegistration =>
            {
                //// Notice we don't have to provide a converter, on WPF a global converter is
                //// registered which knows how to convert a boolean into visibility.
                //this.OneWayBind(ViewModel, viewModel => viewModel.IsAvailable, view => view.searchResultsListBox.Visibility)
                //    .DisposeWith(disposableRegistration);

                // Values
                this.Bind(ViewModel, viewModel => viewModel.StartNumber, view => view.StartNumberNumericUpDown.Value, value => value, value => (int)value).DisposeWith(disposableRegistration);
                this.Bind(ViewModel, viewModel => viewModel.EndNumber, view => view.EndNumberNumericUpDown.Value, value => value, value => (int)value).DisposeWith(disposableRegistration);
                this.OneWayBind(ViewModel, viewModel => viewModel.PrimesCountResultText, view => view.ResultTextBlock.Text).DisposeWith(disposableRegistration);

                // Commands
                this.BindCommand(ViewModel, viewModel => viewModel.CalculatePrimeNumbers, view => view.CalculatePrimeNumbersButton).DisposeWith(disposableRegistration);
            });
        }
    }
}
