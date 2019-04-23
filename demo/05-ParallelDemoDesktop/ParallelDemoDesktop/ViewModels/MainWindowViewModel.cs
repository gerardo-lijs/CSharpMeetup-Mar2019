using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCancelDesktop.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        // Commands
        public ReactiveCommand<Unit, Unit> ProcessImages { get; }
        public ReactiveCommand<Unit, Unit> ProcessImagesParallel { get; }

        // Cancel
        public ReactiveCommand<Unit, Unit> CalculateCancel { get; }
        private CancellationTokenSource _ctsCancel { get; set; }

        private string _ResultText;
        public string ResultText
        {
            get => _ResultText;
            private set => this.RaiseAndSetIfChanged(ref _ResultText, value);
        }

        private readonly ObservableAsPropertyHelper<bool> _IsCalculating;
        public bool IsCalculating => _IsCalculating.Value;

        public MainWindowViewModel()
        {
            // Create command
            ProcessImages = ReactiveCommand.CreateFromTask(ProcessImagesImpl);
            ProcessImagesParallel = ReactiveCommand.CreateFromTask(ProcessImagesImpl);

            // Progress
            _IsCalculating = this.WhenAnyObservable(x => x.ProcessImages.IsExecuting, x => x.ProcessImagesParallel.IsExecuting, (processImages, processImagesParallel) => processImages || processImagesParallel)
                .ToProperty(this, x => x.IsCalculating);

            // Cancel command
            CalculateCancel = ReactiveCommand.Create(() => _ctsCancel?.Cancel(true));
        }

        private async Task ProcessImagesImpl()
        {
            _ctsCancel = new CancellationTokenSource();

            try
            {
                await Task.Delay(5000);

                ResultText = $"Process took xx seconds";
            }
            catch (OperationCanceledException)
            {
                ResultText = "Cancelled";
            }
        }
    }
}
