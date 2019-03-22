# Slides

## Introduction to Asynchronous Programming in .NET
What is Asynchronous Programming

Practical use cases
* Reponsive user interfaces (WPF, WinForms, etc)
* Scalabilty and performance in web applications

## Obsolete patterns
* Background worker
* EAP and APM

## CPU bound vs IO bound
* Blocking vs Callbacks

```csharp
while (true)
  Thread.Sleep(500);
```

```csharp
while (true)
  await Task.Delay(500);
```

* Compute bound operations
Use Task.Run to execute some expensive compute code asyncrhonically

```csharp
Task<int> GetPrimesCountAsync (int start, int count)
{
    return Task.Run (() =>
        ParallelEnumerable.Range (start, count).Count (n =>
            Enumerable.Range (2, (int)Math.Sqrt(n)-1).All (i => n % i > 0)));
}
```

```csharp
int result = Task.Run(() => 
{ 
    int primeNumber = ComputeExpensivePrimeNumberMath(); 
    return primeNumber; 
});
```

* You can also use an async anonymoous method if you need to run asynchronous code

```csharp
int result = await Task.Run(async () => 
{ 
    await Task.Delay(1000); 
    return 42; 
});
```

* Network

## Parallel programming

* Synchronization Contexts
* Thread Pool
* Tasks (Framework 4.5)
* TaskCompletionSource

* Task Cancellation

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
