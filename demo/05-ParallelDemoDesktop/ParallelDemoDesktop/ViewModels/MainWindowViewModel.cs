using OpenCvSharp;
using ReactiveUI;
using System;
using System.Diagnostics;
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
            ProcessImagesParallel = ReactiveCommand.CreateFromTask(ProcessImagesParallelImpl);

            // Progress
            _IsCalculating = this.WhenAnyObservable(x => x.ProcessImages.IsExecuting, x => x.ProcessImagesParallel.IsExecuting, (processImages, processImagesParallel) => processImages || processImagesParallel)
                .ToProperty(this, x => x.IsCalculating);

            // Cancel command
            CalculateCancel = ReactiveCommand.Create(() => _ctsCancel?.Cancel(true));
        }

        private async Task ProcessImagesImpl()
        {
            _ctsCancel = new CancellationTokenSource();

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                await Task.Run(() =>
                {
                    foreach (var filename in System.IO.Directory.EnumerateFiles(Environment.CurrentDirectory, "*.png"))
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            Mat src = new Mat(filename);
                            Mat resut = src.Canny(50, 200);
                        }

                        //using (new Window($"src image {filename}", src))
                        //using (new Window($"resut image {filename}", resut))
                        //{
                        //    //Cv2.WaitKey();
                        //}
                    }
                });

                ResultText = $"Process took {sw.Elapsed.TotalSeconds} seconds";
            }
            catch (OperationCanceledException)
            {
                ResultText = "Cancelled";
            }
        }

        private async Task ProcessImagesParallelImpl()
        {
            _ctsCancel = new CancellationTokenSource();

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(System.IO.Directory.EnumerateFiles(Environment.CurrentDirectory, "*.png"), (filename) =>
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            Mat src = new Mat(filename);
                            Mat resut = src.Canny(50, 200);
                        }

                        //using (new Window($"src image {filename}", src))
                        //using (new Window($"resut image {filename}", resut))
                        //{
                        //    //Cv2.WaitKey();
                        //}
                    });
                });

                ResultText = $"Process took {sw.Elapsed.TotalSeconds} seconds";
            }
            catch (OperationCanceledException)
            {
                ResultText = "Cancelled";
            }
        }
    }
}
