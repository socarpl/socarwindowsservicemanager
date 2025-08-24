public static StartupType GetServiceStartupType(ServiceStartMode startMode)
{
    return startMode switch
    {
        ServiceStartMode.Automatic => StartupType.Automatic,
        ServiceStartMode.Manual => StartupType.Manual,
        ServiceStartMode.Disabled => StartupType.Disabled,
        _ => StartupType.Unknown
    };
}
