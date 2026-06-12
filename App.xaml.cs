using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace VirtualMouse;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += (_, args) =>
        {
            File.WriteAllText("crash.log", args.Exception.ToString());
            args.Handled = true; // 不退出，继续运行
        };

        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex)
                File.WriteAllText("crash.log", ex.ToString());
        };
    }
}
