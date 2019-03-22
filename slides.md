# Slides

## Asynchronous Programming in .NET
* What is Asynchronous Programming
* CPU-bound vs IO-bound
* Obsolete patterns
* Tasks and async/await

Practical use cases
* Reponsive user interfaces (WPF, WinForms, etc)
* Scalabilty and performance in web applications

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
    // We explicitly create a private parameterless constructor so that we are forced to use the static async method
	private ExampleClass() {}		

	public static async Task<ExampleClass> CreateAsync()
    {
		var ret = new ExampleClass();

		// Previous initialization code moved here
		// Plus you can call async methods with await now

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

