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
