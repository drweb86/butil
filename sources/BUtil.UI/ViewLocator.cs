using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BUtil.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.UI;

public class ViewLocator : IDataTemplate
{
    private static readonly Dictionary<Type, Type?> _cache = [];

    public Control Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock { Text = "data was null" };
        }

        var viewModelType = data.GetType();

        if (!_cache.TryGetValue(viewModelType, out var viewType))
        {
            var targetInterface = typeof(IViewLocatorAware<>).MakeGenericType(viewModelType);
            viewType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => { try { return a.GetTypes(); } catch { return []; } })
                .FirstOrDefault(t => !t.IsAbstract && targetInterface.IsAssignableFrom(t));
            _cache[viewModelType] = viewType;
        }

        if (viewType != null)
        {
            return (Control)Activator.CreateInstance(viewType)!;
        }
        else
        {
            return new TextBlock { Text = "Not Found: IViewLocatorAware<" + viewModelType.Name + ">" };
        }
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}