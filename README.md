# C# MeetUp - 30 April 2019
Asynchronous programming in .NET slides and code for this [MeetUp](https://www.meetup.com/meetup-group-bXuvFCuo/events/259826185/)

## Content
* Slides [(Speaker Deck)](https://speakerdeck.com/gerardolijs/asynchronous-programming-in) / [(PDF)](async-programming-slides.pdf)
* [Responsive WPF application demo (Source Code)](https://github.com/gerardo-lijs/Asynchronous-Programming-Samples/tree/master/01-AsyncDesktop)  
Demo source code of a WPF application to demonstrate responsive UI and the beneficts of using async
* [Scalability and performance in web applications demo (Soure Code)](https://github.com/gerardo-lijs/Asynchronous-Programming-Samples/tree/master/02-AsyncWebAPI)  
Demo source code of two Web API to demonstrate performance of using async with IO-bound and CPU-bound operations
* [Progress in a WPF application demo (Source Core)](https://github.com/gerardo-lijs/Asynchronous-Programming-Samples/tree/master/03-AsyncProgressDesktop)  
Demo soure code to demonstrate the simple use of a ProgressRing
* [Cancellation in a WPF application demo (Source Code)](https://github.com/gerardo-lijs/Asynchronous-Programming-Samples/tree/master/04-AsyncCancelDesktop)  
Demo soure code to demonstrate the simple use of CancellationToken
* [Parallel ForEach and Task.WhenAny demo application (Source Code)](https://github.com/gerardo-lijs/Asynchronous-Programming-Samples/tree/master/05-ParallelDemoDesktop)  
Demo soure code of a simple WPF Desktop Application to demonstrate how you can use Task Parallel Library ForEach for CPU-bound operations and how you can use Task.WhenAny for IO-bound operations  

## Notes
* In WPF desktop sample I didn't use MVVM to have a minimalistic example
* In Progress, Cancellation and Parallel samples source code I used [ReactiveUI](https://reactiveui.net/) as MVVM framework, [MahApps](https://mahapps.com/) and [OpenCVSharp](https://github.com/shimat/opencvsharp) for simulating a CPU-bound expensive operation. If you are interested in this projects it's worth checkin it out since they are a minimalistic example and easy to see how they work.
* The demo applications use .NET Core 3 Preview 4 so if you don't have it already you an get it from [here](https://dotnet.microsoft.com/download/dotnet-core/3.0)
* You may need to enable .NET Core 3 in VS2019 [(howto)](https://visualstudiomagazine.com/articles/2019/03/08/vs-2019-core-tip.aspx)

## More resources
Check this [links](https://github.com/gerardo-lijs/Asynchronous-Programming)

# Slides code

## Asynchronous Programming in .NET
* What is Asynchronous Programming
* CPU-bound vs IO-bound
* Obsolete patterns
* Tasks and async/await

Practical use cases
* Responsive user interfaces (WPF, WinForms, etc)
* Scalability and performance in web applications

## Obsolete patterns

* BackgroundWorker
```csharp
backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
```

* Event-Based Asynchronous Pattern (EAP)
```csharp
public void DownloadDataAsync (Uri address);
public event DownloadDataCompletedEventHandler DownloadDataCompleted;
public bool IsBusy { get; } // Indicates if still running
```

* Asynchronous Programming Model (APM)
```csharp
public IAsyncResult BeginRead (byte[] buffer, int offset, int size, AsyncCallback callback, object state);
public int EndRead (IAsyncResult asyncResult);
```

## CPU-bound vs IO-bound
* Blocking vs Callbacks
```csharp
while (true)
  Thread.Sleep(500);
```

## CPU-bound (compute-bound) operations
Use Task.Run to execute expensive CPU-bound code in the ThreadPool

Define a Task returning method
```csharp
Task<int> GetPrimesCountAsync (int start, int count)
{
    return Task.Run (() =>
        ParallelEnumerable.Range (start, count).Count (n =>
            Enumerable.Range (2, (int)Math.Sqrt(n)-1).All (i => n % i > 0)));
}
```

Call method with await
```csharp
int result = await GetPrimesCountAsync (2, 1000000);
```

## Cancelling a CPU-bound method

[TODO: Insert sample here]


## IO-bound operations
* Hard drive

[TODO: Insert sample here with StreamReader]

* Network

* Database

[TODO: Insert sample here with EntityFramework]

## Parallel programming

* Synchronization Contexts
* Thread Pool
* Tasks (Framework 4.5)
* TaskCompletionSource

## Task Cancellation
Use CancellationToken in your method and check manually

```csharp
async Task Foo (CancellationToken cancellationToken)
{
  for (int i = 0; i < 10; i++)
  {
    Console.WriteLine (i);
    await Task.Delay (1000);
    cancellationToken.ThrowIfCancellationRequested();
  }
}
```

Use CancellationTokenSource / Linked

Extension method
```csharp
public static Task<TResult> WithCancellation<TResult> (this Task<TResult> task, CancellationToken cancelToken)
{
    var tcs = new TaskCompletionSource<TResult>();
    var reg = cancelToken.Register (() => tcs.TrySetCanceled ());
    task.ContinueWith (ant =>
    {
        reg.Dispose();
        if (ant.IsCanceled)
            tcs.TrySetCanceled();
        else if (ant.IsFaulted)
            tcs.TrySetException (ant.Exception.InnerException);
        else
            tcs.TrySetResult (ant.Result);
    });
    return tcs.Task;
}
```

[TODO: Insert cample here]

## Run a Task with a Timeout

[TODO: Insert cample here]

Extension method
```csharp
public async static Task<TResult> WithTimeout<TResult> (this Task<TResult> task, TimeSpan timeout)
{
    Task winner = await (Task.WhenAny (task, Task.Delay (timeout)));
    if (winner != task) throw new TimeoutException();
    return await task; // Unwrap result/re-throw
}
```
  
Source: C# in a Nutshell
Source: David Fowler -> AspNetCoreDiagnosticScenarios/Scenarios/Infrastructure/TaskExtensions.cs
      
## Progress reporting

[TODO: Insert cample here]

## Parallel tasks
We can use Task.WhenAny and Task.WhenAll

[TODO: Insert cample here]


* Synchronous Versus Asynchronous Operations

## Web applications
* Improves throughput because of reduced number of threads usage, less memory use and less CPU use

[Insert two graph / before / after]

Source: https://caleblloyd.com/software/net-core-mvc-thread-pool-vs-async/

* Use async EntityFramework methods (using System.Data.Entity namespace)
* Use async for consuming REST API
* Use async for file IO
* Use async for network IO
* Use async for ...


## Running asynchronous method synchronously
Use helper method borrowed from Microsoft

```csharp
public static class AsyncHelper  
{
    private static readonly TaskFactory _taskFactory = new
        TaskFactory(CancellationToken.None,
                    TaskCreationOptions.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.Default);

    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        => _taskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

    public static void RunSync(Func<Task> func)
        => _taskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
}
```

Use like this

```csharp
var result = AsyncHelper.RunSync(() => DoAsyncStuff());  
```

Source: https://cpratt.co/async-tips-tricks/


## Async class constructors
* They are not available by default in the framework.
* Refactor with InitializeAsync method

Previous class
```csharp
public class ExampleClass
{
	public ExampleClass()
	{
		// Current initialization code here
	}
}
```

Refactored class
```csharp
public class ExampleClass
{
	public ExampleClass()
	{
		// No more code here
	}

	public async Task InitializeAsync()
	{
		// Previous initialization code moved here
		// Plus you can call async methods with await now
		// Problem -> Consumers need to know they have to always run this method after creating class
    // It's better to use Factory Pattern in my opinion
	}
}
```

You now have to use your class like this
```csharp
    var classInstance = new ExampleClass();
    await classInstance.InitializeAsync();
```

* Refactor with Factory Pattern

Previous class
```csharp
public class ExampleClass
{
    public ExampleClass()
    {
        // Current initialization code here
    }
}
```

Refactored class
```csharp
public class ExampleClass
{
    // We explicitly create a private parameterless constructor 
    // so that we are forced to use the static async method
    private ExampleClass() {}		

    private async Task InitializeAsync()
    {
        // Previous initialization code moved here
        // Plus you can call async methods with await now
    }

    public static async Task<ExampleClass> CreateAsync()
    {
        var ret = new ExampleClass();
        await ret.InitializeAsync();
        return ret;
    }
}
```

You now have to use your class like this
```csharp
    var classInstance = await ExampleClass.CreateAsync();
```

* Many other options from Stephen Cleary
[Async Constructors by Stephen Cleary (2013)](http://blog.stephencleary.com/2013/01/async-oop-2-constructors.html)

* Use AsyncHelper.RunSync which works quite well most of the times without Blocking but you loose all the beneficts of async code

## Exception Handling
