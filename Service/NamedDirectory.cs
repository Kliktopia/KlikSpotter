namespace KlikSpotter.Service;

internal readonly struct NamedDirectory
{
    public DirectoryType Type { get; init; }
    public string Path { get; init; }

    [SetsRequiredMembers]
    public NamedDirectory(DirectoryType type)
    {
        Type = type;
        Path = Type switch
        {
            DirectoryType.Found => "!FOUND",
            DirectoryType.Failed => "!PROBLEMATIC",
            DirectoryType.Moved => "!MOVED",
            DirectoryType.Indeterminate => "!UNKNOWN",
            DirectoryType.KlikNPlay => "!KNP_APPS",
            DirectoryType.TheGamesFactory => "!TGF_APPS",
            DirectoryType.ClickNCreate => "!CNC_APPS",
            DirectoryType.MultimediaFusion => "!MMF_APPS",
            DirectoryType.InstallCreatorInstaller => "!IC_INSTALLERS",
            DirectoryType.InstallMakerInstaller => "!IM_INSTALLERS",
            DirectoryType.KlikNPlayInstaller => "!KNP_INSTALLERS",
            DirectoryType.ClickNCreateInstaller => "!CNC_INSTALLERS",
            DirectoryType.TheGamesFactoryInstaller => "!TGF_INSTALLERS",
            _ => throw new NotImplementedException()
        };
    }
}
