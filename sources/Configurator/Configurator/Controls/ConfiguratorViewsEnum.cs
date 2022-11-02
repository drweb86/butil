namespace BUtil.Configurator.Configurator.Controls
{
	/// <summary>
	/// Enum for switching between configurator views. Order isn't significant
	/// </summary>
	internal enum ConfiguratorViewsEnum
	{
        Tasks,
		OtherOptions,
		Logging
	}

    /// <summary>
    /// Enum for switching between different tasks
    /// </summary>
    internal enum BackupTaskViewsEnum
    {
        Name,
        SourceItems,
        How,
        Storages,
        Scheduler,
        Encryption,
        OtherOptions,
    }
}
