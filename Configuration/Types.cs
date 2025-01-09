namespace KlikSpotter.Configuration;

internal enum DirectoryType
{
    None = default,
    Found,
    Failed,
    Moved,
    Indeterminate,
    KlikNPlay,
    TheGamesFactory,
    ClickNCreate,
    MultimediaFusion,
    KlikNPlayInstaller,
    ClickNCreateInstaller,
    TheGamesFactoryInstaller,
    InstallCreatorInstaller,
    InstallMakerInstaller,
}

internal enum MatchIndicator
{
    Unset = default,

    KlikNPlay,
    TheGamesFactory,
    ClickNCreate,

    MultimediaFusion,
    MultimediaFusion10,
    MultimediaFusion12,
    MultimediaFusion15,
    MultimediaFusion20,
    MultimediaFusionExpress,

    Installer,
    InstallCreatorInstaller,
    InstallMakerInstaller,
    InstallMakerProInstaller,
    KlikNPlayInstaller,
    ClickNCreateInstaller,
    TheGamesFactoryInstaller,

    Vitalize,
}

[Flags]
enum FileType
{
    All = default,
    Executable = 1,
    Archive = 2,
    Application = 4
}
