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
* Network

## Parallel programming

* Synchronization Contexts
* Thread Pool
* Tasks (Framework 4.5)
* TaskCompletionSource

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
