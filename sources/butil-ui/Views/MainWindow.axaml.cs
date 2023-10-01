using Avalonia.Controls;
using BUtil.Core.FileSystem;
using System;
using System.Diagnostics;
using System.Linq;

namespace butil_ui.Views;

public partial class MainWindow : Window
{
    public Avalonia.Controls.UserControl CurrentView { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        if (args.Length == 2 && args[0].ToUpperInvariant() == TasksAppArguments.LaunchTask.ToUpperInvariant())
        {
            string taskName = null;
            foreach (var argument in args)
            {
                if (argument.StartsWith(TasksAppArguments.RunTask) && argument.Length > TasksAppArguments.RunTask.Length)
                {
                    taskName = argument.Substring(TasksAppArguments.RunTask.Length + 1);
                }
            }
            CurrentView = new LaunchTaskView();
        }
    }
}
