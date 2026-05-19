# Custom Task Plugin

BUtil supports custom task assemblies. A task plugin provides the runtime task logic, and an optional separate task UI plugin can provide the create/edit screen shown in the BUtil desktop app.

Both plugin types compile against **`BUtil.Interop`**. Runtime task plugins are loaded by the core app; task UI plugins are loaded only by the desktop UI.

## Where To Start

The easiest way to create a custom task is to copy one of the built-in task projects and trim it down:

- Runtime examples: `sources/BUtil.Tasks.BUtilClient` and `sources/BUtil.Tasks.BUtilServer`
- UI examples: `sources/BUtil.Tasks.BUtilClient.UI` and `sources/BUtil.Tasks.BUtilServer.UI`

For the smallest registration examples, look at:

- `BUtilClientTaskPlugin.cs`
- `BUtilClientTaskUIPlugin.cs`

## Runtime Task Plugin

A runtime task plugin is a .NET class library that:

1. Defines a task options model implementing `ITaskModelOptionsV2`.
2. Defines a task implementation by deriving from `BuTask` or `BuTaskV2`.
3. Exposes a public concrete `ITaskPlugin` class whose `Register()` method calls `TaskProviderRegistry.Register(...)`.

Minimal shape:

```csharp
using BUtil.Interop.Logs;
using BUtil.Interop.Tasks;
using BUtil.Interop.Tasks.Core;
using BUtil.Interop.Tasks.Events;

public sealed class MyTaskOptions : ITaskModelOptionsV2
{
    public string Name { get; set; } = string.Empty;
}

public sealed class MyRootTask(ILog log, TaskEvents events, TaskV2 task)
    : BuTaskV2(log, events, "My task")
{
    protected override void ExecuteInternal()
    {
        var options = (MyTaskOptions)task.Model;
        LogDebug($"Running {options.Name}");
    }
}

public sealed class MyTaskPlugin : ITaskPlugin
{
    public void Register()
    {
        TaskProviderRegistry.Register<MyTaskOptions>(
            jsonType: "MyTask",
            information: "Runs my custom task.",
            factory: (log, task, events, onMsg) => new MyRootTask(log, events, task),
            verifier: (log, options, writeMode) => null);
    }
}
```

Place the compiled runtime plugin DLL and its dependencies in one of:

- Portable install: `plugins/tasks` next to the application binaries
- Per-user install: `%AppData%\BUtil\plugins\tasks\` on Windows
- Debug builds use `%AppData%\BUtil-Development\plugins\tasks\`

## Optional Task UI Plugin

Without a UI plugin, the task can still be loaded/run if a configuration already exists, but the desktop app cannot create or edit it.

A task UI plugin is a separate .NET class library that:

1. References `BUtil.Interop`.
2. References Avalonia if it returns Avalonia controls.
3. References the task runtime assembly so it can use the same model type.
4. Exposes a public concrete `ITaskUIPlugin` class whose `Register()` method calls `TaskUIProviderRegistry.Register(...)`.

Task UI plugins use the namespace `BUtil.Interop.Tasks.UI`.

Recommended shape for external plugins: return an Avalonia `UserControl` from the factories. That control can set its own `DataContext` internally, so it does **not** need to inherit from BUtil UI base classes.

```csharp
using Avalonia.Controls;
using BUtil.Interop.Tasks.UI;

public sealed class MyTaskEditor : UserControl
{
    public MyTaskEditor(string title, bool isNew)
    {
        Content = new TextBlock { Text = isNew ? $"Create {title}" : $"Edit {title}" };
    }
}

public sealed class MyTaskUIPlugin : ITaskUIPlugin
{
    public void Register()
    {
        TaskUIProviderRegistry.Register<MyTaskOptions>(
            createNewFactory: () => new MyTaskEditor("My task", isNew: true),
            editFactory: taskName => new MyTaskEditor(taskName, isNew: false),
            createHeader: "My task",
            group: "custom",
            preferredOrder: 100);
    }
}
```

`createHeader` is shown in the **Task > Create** menu. `group` groups related task types together; BUtil inserts separators between groups. `preferredOrder` sorts items inside a group.

Place the compiled UI plugin DLL and its dependencies in one of:

- Portable install: `plugins/task-uis` next to the application binaries
- Per-user install: `%AppData%\BUtil\plugins\task-uis\` on Windows
- Debug builds use `%AppData%\BUtil-Development\plugins\task-uis\`

## Notes

- The task runtime plugin and task UI plugin may be separate assemblies. This is recommended so command-line or service usage does not need Avalonia dependencies.
- The model type used in `TaskUIProviderRegistry.Register<TModel>()` must be the same CLR type registered by `TaskProviderRegistry.Register<TModel>()`.
- Built-in task UI projects may inherit BUtil UI base classes because they are part of the app source tree. External plugins should prefer returning Avalonia controls and referencing `BUtil.Interop` only.
- The `jsonType` passed to `TaskProviderRegistry.Register` is persisted in task JSON. Keep it stable across plugin versions.
