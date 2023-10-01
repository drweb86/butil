using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BUtil.Core.FileSystem;
using butil_ui.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;

namespace butil_ui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var viewModel = new MainWindowViewModel();
        
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
            viewModel.SelectedView = TasksAppArguments.RunTask;
            viewModel.SelectedTask = taskName;
        }

        this.DataContext = viewModel;
    }
}
