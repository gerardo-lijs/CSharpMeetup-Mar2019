# C# MeetUp - 30 April 2019
Asynchronous programming in .NET slides and code for this [MeetUp](https://www.meetup.com/meetup-group-bXuvFCuo/events/259826185/)

Content
* [Slides (Speaker Deck)](https://speakerdeck.com/gerardolijs/asynchronous-programming-in)
* [Slides (PDF)](slides.pdf)
* [Responsive WPF application demo (Source Code)](demo/01-AsyncDesktop)  
Demo source code of a WPF application to demonstrate responsive UI and the beneficts of using async
* [Scalability and performance in web applications demo (Soure Code)](demo/02-AsyncWebAPI)  
Demo source code of two Web API to demonstrate performance of using async with IO-bound and CPU-bound operations
* [Parallel ForEach and Task.WhenAny demo application (Source Code)](demo/05-ParallelDemoDesktop)  
Demo soure code of a simple WPF Desktop Application to demonstrate how you can use Task Parallel Library ForEach for CPU-bound operations and how you can use Task.WhenAny for IO-bound operations  
In this source code I used [ReactiveUI](https://reactiveui.net/) as MVVM framework, [MahApps](https://mahapps.com/) and [OpenCVSharp](https://github.com/shimat/opencvsharp) for simulating a CPU-bound expensive operation. If you are interested in this projects it's worth checkin it out since they are a minimalistic example and easy to see how they work.

Notes
  * The demo applications use .NET Core 3 Preview 4 so if you don't have it already you an get it from [here](https://dotnet.microsoft.com/download/dotnet-core/3.0)
  * You may need to enable .NET Core 3 in VS2019 [(howto)](https://visualstudiomagazine.com/articles/2019/03/08/vs-2019-core-tip.aspx)
