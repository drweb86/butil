using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BUtil.Core.FileSystem;
using butil_ui.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;

namespace butil_ui.Views;

// 1. ListBox должен показываться если на него хватает места.
// 2. Минимальная высота.
// 3. Поломанные биндинги к цветам
// 4. Иконки для кнопок должны вернуться.
// 5. Иконки должны быть из FLuent 2 чтобы соответствовать винде.
// 6. Диалоговые окна надо бы вернуть
// 7. Концепт backupTask => task в коде и переименования
// 8. Нужно улучшить читаемость и XAML тяжело воспринять. Нужно стили 
// 9. Иконка

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
