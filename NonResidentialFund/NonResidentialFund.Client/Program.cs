using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;

namespace NonResidentialFund.Client;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(exception =>
            {
                Debug.WriteLine(exception);
            });
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
