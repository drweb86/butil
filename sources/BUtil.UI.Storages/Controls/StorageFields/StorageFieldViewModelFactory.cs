using BUtil.Core.Storages;
using System;

namespace BUtil.UI.Controls.StorageFields;

public static class StorageFieldViewModelFactory
{
    public static StorageFieldViewModel Create(StorageFieldDescriptor descriptor) => descriptor.Type switch
    {
        StorageFieldType.Text => new TextFieldViewModel(descriptor),
        StorageFieldType.Password => new PasswordFieldViewModel(descriptor),
        StorageFieldType.Integer => new IntegerFieldViewModel(descriptor),
        StorageFieldType.Folder => new FolderFieldViewModel(descriptor),
        StorageFieldType.File => new FileFieldViewModel(descriptor),
        StorageFieldType.Enum => new EnumFieldViewModel(descriptor),
        _ => throw new ArgumentOutOfRangeException(nameof(descriptor), descriptor.Type, null),
    };
}
