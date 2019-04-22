using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AsyncProgressDesktop.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        // Commands
        public ReactiveCommand<Unit, Unit> CalculatePrimeNumbers { get; }

        private int _StartNumber = 1;
        public int StartNumber
        {
            get => _StartNumber;
            set => this.RaiseAndSetIfChanged(ref _StartNumber, value);
        }

        private int _EndNumber = 5000000;
        public int EndNumber
        {
            get => _EndNumber;
            set => this.RaiseAndSetIfChanged(ref _EndNumber, value);
        }

        private string _PrimesCountResultText;
        public string PrimesCountResultText
        {
            get => _PrimesCountResultText;
            private set => this.RaiseAndSetIfChanged(ref _PrimesCountResultText, value);
        }

        private bool _IsCalculating;
        public bool IsCalculating
        {
            get => _IsCalculating;
            private set => this.RaiseAndSetIfChanged(ref _IsCalculating, value);
        }

        public MainWindowViewModel()
        {
            // Create command
            CalculatePrimeNumbers = ReactiveCommand.CreateFromTask(CalculatePrimeNumbersImpl);

            //_isAvailable = this
            //           .WhenAnyValue(x => x.SearchResults)
            //           .Select(searchResults => searchResults != null)
            //           .ToProperty(this, x => x.IsAvailable);
        }

        private async Task CalculatePrimeNumbersImpl()
        {
            // Notify UI that calculation is in progress
            IsCalculating = true;

            // Calculate
            int primesCount = await Task.Run(() =>
                 ParallelEnumerable.Range(StartNumber, EndNumber).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

            PrimesCountResultText = $"{primesCount} prime numbers between {StartNumber} and {EndNumber}";

            // Notify UI that calculation is finished
            IsCalculating = false;
        }
    }
}
