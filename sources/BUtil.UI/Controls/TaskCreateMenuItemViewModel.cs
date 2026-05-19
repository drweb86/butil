using BUtil.Interop.UI.Tasks;
using System;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public sealed class TaskCreateMenuItemViewModel
{
    private readonly Type _modelType;

    public TaskCreateMenuItemViewModel(Type modelType, string header)
    {
        _modelType = modelType;
        Header = header;
        CreateCommand = new DelegateCommand(Create);
    }

    public string Header { get; }
    public ICommand CreateCommand { get; }

    private void Create()
    {
        var viewModel = TaskUIProviderRegistry.CreateNew(_modelType);
        if (viewModel != null)
            WindowManager.SwitchTaskUIView(viewModel);
    }
}

public sealed class TaskCreateMenuSeparatorViewModel
{
}

internal sealed class DelegateCommand(Action execute) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => execute();
}
