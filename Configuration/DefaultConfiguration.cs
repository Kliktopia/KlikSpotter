namespace KlikSpotter.Configuration;

internal static class DefaultConfiguration
{
    public static readonly IEnumerable<NamedDirectory> DirectoryNames =
    [
        new(DirectoryType.Found),
        new(DirectoryType.Indeterminate),
        new(DirectoryType.Failed),
        new(DirectoryType.Moved),
        new(DirectoryType.KlikNPlayInstaller),
        new(DirectoryType.TheGamesFactoryInstaller),
        new(DirectoryType.ClickNCreateInstaller),
        new(DirectoryType.InstallMakerInstaller),
        new(DirectoryType.InstallCreatorInstaller),
    ];

    public static readonly IEnumerable<FileExtension> FileExtensions =
    [
        new(".exe"),
        new(".cca", isArchive: false, [MatchIndicator.ClickNCreate]),
        new(".ccn", isArchive: false, [MatchIndicator.Vitalize]),
        new(".hts", isArchive: false, [MatchIndicator.Vitalize]),
        new(".gam", isArchive: false, [MatchIndicator.TheGamesFactory]),
        new(".mfa", isArchive: false, [MatchIndicator.MultimediaFusion]),
        new(".zip", isArchive: true),
        new(".zipx", isArchive: true),
        new(".cbz", isArchive: true),
        new(".rar", isArchive: true),
        new(".r00", isArchive: true),
        new(".cbr", isArchive: true),
        new(".7z", isArchive: true),
        new(".tar", isArchive: true),
        new(".gz", isArchive: true),
        new(".bz2", isArchive: true),
        new(".taz", isArchive: true),
        new(".tgz", isArchive: true),
        new(".tb2", isArchive: true),
        new(".tbz", isArchive: true),
        new(".tbz2", isArchive: true),
        new(".tz2", isArchive: true),
    ];


    public static readonly IEnumerable<string> FilesThatIndicatesMatch =
    [
        "cctrans.dll",
        "cncs.dll",
        "CNCS216.DLL",
        "cncs232.dll",
        "cncs32.dll",
        "knpg.dll",
        "knpres.dll",
        "knps.dll",
        "mmfs2.dll",
        "modfx.dll",
        "rubberovine.dll",
    ];

    public static readonly IEnumerable<string> FilesToRemove =
    [
        "cncs.dll",
        "CNCS216.DLL",
        "cncs232.dll",
        "cncs32.dll",
        "knpg.dll",
        "knpres.dll",
        "knps.dll",
        "mmfs2.dll",
        "uninst32.bin",
        "uninst32.exe",
        "uninstal.bin",
        "uninstal.exe",
        "uninstall.bin",
        "uninstall.exe",
    ];

    public static readonly IEnumerable<SearchPattern> SearchPatterns =
    [
        new
        (
            alias: "CNCS232",
            data: "CNCS232",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators =
            [
                MatchIndicator.ClickNCreate,
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "INSTALL_MAKER",
            data: [119, 119, 103, 84, 41, 72, 0, 0, 0, 0]
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.Installer,
                MatchIndicator.InstallMakerInstaller
            ]
        },
        new
        (
            alias: "INSTALL_CREATOR",
            data: [119, 119, 103, 84, 41, 72]
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.Installer,
                MatchIndicator.InstallCreatorInstaller
            ]
        },
        new
        (
            alias: "KNP_INSTALLER",
            data: "Klik & Play stand alone installer",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.KlikNPlay,
                MatchIndicator.KlikNPlayInstaller
            ]
        },
        new
        (
            alias: "TGF_INSTALLER",
            data: "GFSAS\0\0",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.Installer,
                MatchIndicator.TheGamesFactory,
                MatchIndicator.TheGamesFactoryInstaller
            ]
        },
        new
        (
            alias: "TGF_INSTALLER_ALT",
            data: "GFSAS32.EXE",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.Installer,
                MatchIndicator.TheGamesFactory,
                MatchIndicator.TheGamesFactoryInstaller
            ]
        },
        new
        (
            alias: "CNC_INSTALLER",
            data: "C&C Stand Alone App Installer",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.Installer,
                MatchIndicator.ClickNCreate,
                MatchIndicator.ClickNCreateInstaller
            ]
        },
        new
        (
            alias: "CNCSA.BIN",
            data: "CNCSA.BIN",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.ClickNCreate,
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "Multimedia Fusion Express",
            data: "Multimedia Fusion Express",
            isUnicode: false

        )
        {
            Priority = 10,
            Indicators =
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusionExpress
            ]
        },
        new
        (
            alias: "cncrt32.exe (Unicode)",
            data: "cncrt32.exe",
            isUnicode: true
        )
        {
            Priority = 10
        },
        new
        (
            alias: "CNCRT32.EXE (Unicode)",
            data: "CNCRT32.EXE",
            isUnicode: true
        )
        {
            Priority = 5
        },
        new
        (
            alias: "cncsa.bin",
            data: "cncsa.bin",
            isUnicode: false
        )
        {
            Priority = 10
        },
        new
        (
            alias: "mmf2d3d8.dll",
            data: "mmf2d3d8.dll",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators =
            [
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "Click & Create",
            data: "Click & Create",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators =
            [
                MatchIndicator.ClickNCreate
            ]
        },
        new
        (
            alias: "<description>Application</description> (Unicode)",
            data: "<description>Application</description>",
            isUnicode: true
        )
        { Priority = 20 },
        new
        (
            alias: "MMFEXPRESS_ICON",
            data: [40, 0, 0, 0, 16, 0, 0, 0, 32, 0, 0, 0, 1, 0, 4, 0, 0, 0, 0, 0, 128, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 0, 0, 128, 0, 0, 0, 128, 128, 0, 128, 0, 0, 0, 128, 0, 128, 0, 128, 128, 0, 0, 192, 192, 192, 0, 128, 128, 128, 0, 0, 0, 255, 0, 0, 255, 0, 0, 0, 255, 255, 0, 255, 0, 0, 0, 255, 0, 255, 0, 255, 255, 0, 0, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 120, 0, 135, 112, 0, 0, 7, 128, 68, 68, 0, 8, 112, 0, 116, 68, 68, 68, 68, 64, 7, 7, 68, 68, 68, 68, 68, 68, 0, 4, 68, 68, 64, 68, 68, 68, 64, 4, 68, 68, 64, 244, 68, 68, 64, 68, 68, 68, 64, 255, 68, 68, 68, 4, 68, 68, 64, 247, 68, 68, 64, 4, 68, 68, 64, 116, 68, 68, 71, 0, 68, 68, 64, 68, 68, 68, 112, 0, 4, 68, 68, 68, 68, 64, 0, 0, 0, 0, 68, 68, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 120, 68, 255, 255, 68, 68, 248, 7, 68, 68, 224, 1, 8, 112, 192, 0, 68, 68, 128, 0, 68, 68, 128, 0, 68, 68, 128, 0, 0, 135, 0, 0, 68, 68, 128, 0, 72, 68, 128, 0, 68, 68, 192, 1, 0, 8, 224, 7, 68, 68, 252, 63, 64, 132, 255, 255, 68, 68, 255, 255, 64, 0]
        )
        {
            Priority = 99,
            Indicators =
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusionExpress
            ]
        },
        new
        (
            alias: "mmfs2.dll (Unicode)",
            data: "mmfs2.dll",
            isUnicode: true
        )
        {
            Priority = 5,
            Indicators =
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "mmf2d3d11.dll (Unicode)",
            data: "mmf2d3d11.dll",
            isUnicode: true
        )
        {
            Priority = 5,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "cncsa32.EXE",
            data: "cncsa32.EXE",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators =
            [
                MatchIndicator.ClickNCreate,
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "lnchrt.exe (Unicode)",
            data: "lnchrt.exe",
            isUnicode: true
        )
        {
            Priority = 5
        },
        new
        (
            alias: "CncMainClass",
            data: "CncMainClass",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.ClickNCreate
            ]
        },
        new
        (
            alias: "MMFS2.dll",
            data: "MMFS2.dll",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "Cannot create subdirectory in temporary directory ! (Unicode)",
            data: "Cannot create subdirectory in temporary directory !",
            isUnicode: true
        )
        {
            Priority = 20
        },
        new
        (
            alias: "The Games Factory Application Runtime (Unicode)",
            data: "The Games Factory Application Runtime",
            isUnicode: true
        )
        {
            Priority = 20,
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ]
        },
        new
        (
            alias: "Multimedia Fusion Application Runtime (Unicode)",
            data: "Multimedia Fusion Application Runtime",
            isUnicode: true
        )
        {
            Priority = 20,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "Klik & Play",
            data: "Klik & Play",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators = 
            [
                MatchIndicator.KlikNPlay
            ]
        },
        new
        (
            alias: "1998 IMSI (Unicode)",
            data: "1998 IMSI",
            isUnicode: true
        )
        {
            Priority = 5
        },
        new
        (
            alias: "mmfs2.dll",
            data: "mmfs2.dll",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "MMFUSION",
            data: "MMFUSION",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "CNCRT32.EXE",
            data: "CNCRT32.EXE",
            isUnicode: false
        )
        {
            Priority = 5,
        },
        new
        (
            alias: "mmf2d3d9.dll (Unicode)",
            data: "mmf2d3d9.dll",
            isUnicode: true
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "mmf2d3d9.dll",
            data: "mmf2d3d9.dll",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "cncrt32.exe",
            data: "cncrt32.exe",
            isUnicode: false
        )
        {
            Priority = 10,
        },
        new
        (
            alias: "mmf2d3d8.dll (Unicode)",
            data: "mmf2d3d8.dll",
            isUnicode: true
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "Invalid data in executable file ! (Unicode)",
            data: "Invalid data in executable file !",
            isUnicode: true
        )
        {
            Priority = 20
        },
        new
        (
            alias: "MAGIC_ALT",
            data: [192, 8, 20, 144, 0, 0, 0, 0, 1, 0, 6, 0, 15, 0, 152, 0, 22, 0, 0, 0, 0, 0]
        )
        {
            Priority = 30
        },
        new
        (
            alias: "Cc2MainClass",
            data: "Cc2MainClass",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "GFactory",
            data: "GFactory",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ]
        },
        new
        (
            alias: "mmf2d3d11.dll",
            data: "mmf2d3d11.dll",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ]
        },
        new
        (
            alias: "Cc2EditClass",
            data: "Cc2EditClass",
            isUnicode: false
        )
        {
            Priority = 10,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "CNCSA32.EXE",
            data: "CNCSA32.EXE",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators = 
            [
                MatchIndicator.ClickNCreate, 
                MatchIndicator.MultimediaFusion
            ]
        },
        new
        (
            alias: "MAGIC",
            data: [192, 0, 20, 144, 0, 0, 0, 0, 1, 0, 6, 0, 15, 0, 152, 0, 22, 0, 0, 0, 0, 0]
        )
        {
            Priority = 30
        },
        new
        (
            alias: "CncEditClass",
            data: "CncEditClass",
            isUnicode: false
        )
        {
            Priority = 5,
            Indicators = 
            [
                MatchIndicator.ClickNCreate
            ]
        },
        new
        (
            alias: "KNP_ICON",
            data: [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x44, 0x44, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x44, 0x4c, 0xcc, 0xcc, 0xcc, 0x44, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x44, 0xcc, 0xcc, 0xcc, 0xcc, 0xcc, 0x44, 0x0b, 0xb0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0xcc, 0xcc, 0xcc, 0xcc, 0xcc, 0xcc, 0x44, 0x0b, 0xb0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4c, 0xcc, 0xcc, 0xcc, 0xc6, 0xcc, 0xcc, 0x44, 0x0b, 0xb0, 0x40, 0x00, 0x00, 0x00, 0x00, 0x04, 0xcc, 0xcc, 0xc6, 0x66, 0x66, 0x66, 0xcc, 0x44, 0x0b, 0xb0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4c, 0xcc, 0xcc, 0x66, 0x66, 0x66, 0x6c, 0xcc, 0x44, 0x0b, 0xbb, 0xbb, 0xbb, 0x00, 0x00, 0x04, 0xcc, 0xcc, 0xc6, 0x66, 0x6c, 0xcc, 0xcc, 0xcc, 0x44, 0x0b, 0xb0, 0x00, 0x0b, 0xb0, 0x00, 0x04, 0xcc, 0xcc, 0x66, 0x66, 0xcc, 0xcc, 0xcc, 0xcc, 0x44, 0x0b, 0xb0, 0x64, 0x0b, 0xb0, 0x00, 0x4c, 0xcc, 0xcc, 0x66, 0xcc, 0xcc, 0x44, 0x44, 0x4c, 0x44, 0x0b, 0xb0, 0x64, 0x0b, 0xb0, 0x00, 0x4c, 0xcc, 0xc6, 0x6f, 0xcc, 0xc4, 0x00, 0x00, 0x00, 0x64, 0x0b, 0xb0, 0x00, 0x0b, 0xb0, 0x00, 0x4c, 0xcc, 0x66, 0x6c, 0xcc, 0x40, 0xbb, 0xbb, 0xbb, 0x0c, 0x0b, 0xbb, 0xbb, 0xbb, 0x00, 0x04, 0xcc, 0xc6, 0x66, 0xfc, 0xc4, 0x0b, 0xb0, 0x0b, 0xb0, 0x4c, 0xc0, 0x00, 0x00, 0x00, 0xc4, 0x04, 0xcc, 0xc6, 0x66, 0xcc, 0xc4, 0x0b, 0xb0, 0x0b, 0xbb, 0x0c, 0xcc, 0xcc, 0xcc, 0xcc, 0xc4, 0x04, 0xcc, 0xc6, 0x6f, 0xcc, 0xc4, 0x0b, 0xb0, 0xbb, 0x0b, 0x0c, 0xcc, 0xcf, 0x6c, 0xcc, 0xc4, 0x04, 0xcc, 0xc6, 0x6f, 0xcc, 0xcc, 0x40, 0xbb, 0xb0, 0x40, 0x00, 0xcc, 0x6f, 0x66, 0xcc, 0xc4, 0x04, 0x44, 0x46, 0x64, 0x44, 0xcc, 0xc4, 0x0b, 0xbb, 0x0c, 0xcc, 0xc6, 0x6f, 0x66, 0xcc, 0xc4, 0x04, 0x44, 0x44, 0x64, 0x44, 0x4c, 0xc0, 0xbb, 0x0b, 0xb0, 0xcc, 0x66, 0x6f, 0x66, 0xcc, 0xc4, 0x04, 0x44, 0x00, 0x44, 0x44, 0x00, 0xc0, 0xbb, 0x0b, 0xb0, 0xcc, 0x66, 0x6f, 0x66, 0xcc, 0xc4, 0x04, 0x40, 0xbb, 0x04, 0x40, 0xbb, 0x00, 0xbb, 0x0b, 0xb0, 0xcc, 0x66, 0xf6, 0x6c, 0xcc, 0x40, 0x04, 0x40, 0xbb, 0x04, 0x0b, 0xb0, 0xcc, 0x0b, 0xbb, 0x0c, 0xc6, 0x66, 0xf6, 0x6c, 0xcc, 0x40, 0x04, 0x40, 0xbb, 0x00, 0xbb, 0x0c, 0xcc, 0xc0, 0x00, 0xcc, 0x66, 0x6f, 0x66, 0xcc, 0xcc, 0x40, 0x04, 0x40, 0xbb, 0x0b, 0xb0, 0xcc, 0xcc, 0xcc, 0xcc, 0x66, 0x66, 0x6f, 0x66, 0xcc, 0xc4, 0x00, 0x04, 0x40, 0xbb, 0xbb, 0x0c, 0xcc, 0xcc, 0x66, 0x66, 0x66, 0x6f, 0xf6, 0x6c, 0xcc, 0xc4, 0x00, 0x04, 0x40, 0xbb, 0xb0, 0xcc, 0xcc, 0xcf, 0x66, 0x66, 0x6f, 0xf6, 0x6c, 0xcc, 0xcc, 0x40, 0x00, 0x04, 0x40, 0xbb, 0xbb, 0x0c, 0xcc, 0x66, 0xff, 0xff, 0xf6, 0x66, 0xcc, 0xcc, 0xc4, 0x00, 0x00, 0x04, 0x40, 0xbb, 0x0b, 0xb0, 0xc6, 0x66, 0x66, 0x66, 0x66, 0x6c, 0xcc, 0xcc, 0x40, 0x00, 0x00, 0x04, 0x40, 0xbb, 0x00, 0xbb, 0x0c, 0x66, 0x66, 0x66, 0xcc, 0xcc, 0xcc, 0xc4, 0x00, 0x00, 0x00, 0x00, 0x40, 0xbb, 0x00, 0x0b, 0xb0, 0xcc, 0xcc, 0xcc, 0xcc, 0xcc, 0xc4, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x4c, 0xcc, 0xcc, 0xcc, 0x44, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x44, 0x44, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]
        )
        {
            Priority = 99,
            Indicators = 
            [
                MatchIndicator.KlikNPlay
            ]
        },
        new
        (
            alias: "TGF_ICON",
            data: [0x00, 0x44, 0x4e, 0xce, 0xee, 0xe4, 0x4c, 0x40, 0x04, 0xcc, 0xef, 0xff, 0xff, 0xfe, 0xec, 0x00, 0x4e, 0xcf, 0xf4, 0x44, 0x6e, 0xff, 0xfc, 0x44, 0xce, 0xef, 0xfc, 0xc4, 0x4e, 0xfe, 0xfe, 0xe0, 0x4e, 0xfe, 0xf4, 0x44, 0x44, 0x4e, 0xff, 0xec, 0x0f, 0xfe, 0xcc, 0xc4, 0x44, 0x4e, 0xff, 0xf0, 0x0f, 0xfe, 0xc6, 0xf4, 0x44, 0x4e, 0xff, 0xfe, 0x0f, 0xfe, 0xc6, 0xf4, 0x44, 0x46, 0x6f, 0xf4, 0x4f, 0xfe, 0xc6, 0x64, 0x44, 0x44, 0x6f, 0xf4, 0x4e, 0xfe, 0xf4, 0x44, 0xf4, 0x4e, 0xff, 0xe4, 0x4e, 0xee, 0xfe, 0xfe, 0xf4, 0x46, 0x6e, 0xe0, 0x4c, 0xef, 0xff, 0xff, 0xfc, 0x44, 0x6e, 0x40, 0x0e, 0xee, 0xff, 0xfe, 0xfe, 0xff, 0xee, 0xc0, 0x04, 0x4c, 0xee, 0xff, 0xff, 0xee, 0xe4, 0xc0, 0x00, 0x0c, 0xc4, 0xcc, 0xcc, 0xc0, 0x44, 0x40, 0x8f, 0xf7, 0x00, 0x07, 0x00, 0x0f, 0x00, 0x00]
        )
        {
            Priority = 99,
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ]
        },
        new
        (
            alias: "CNC_ICON",
            data: [0x00, 0x00, 0x87, 0x88, 0x87, 0x78, 0x80, 0x00, 0x00, 0x07, 0x84, 0x44, 0x44, 0x47, 0x70, 0x00, 0x00, 0x74, 0x44, 0x44, 0x44, 0x44, 0x88, 0x80, 0x07, 0x84, 0x44, 0x44, 0x44, 0x44, 0x47, 0x70, 0x04, 0x44, 0x44, 0x44, 0x44, 0x33, 0x34, 0x80, 0x03, 0x33, 0x34, 0x33, 0x4b, 0x34, 0x43, 0x88, 0x73, 0x34, 0xbb, 0x3b, 0x3b, 0x34, 0x44, 0x48, 0x73, 0x34, 0x44, 0xb3, 0x3b, 0x34, 0x44, 0x48, 0x73, 0x34, 0x44, 0xb3, 0x3b, 0x34, 0x43, 0x48, 0x03, 0x34, 0xb4, 0x4b, 0x44, 0xb3, 0x34, 0x80, 0x0b, 0x33, 0x34, 0x44, 0x44, 0x44, 0x44, 0x80, 0x07, 0x84, 0x4c, 0xc4, 0x44, 0x44, 0x47, 0x70, 0x00, 0x74, 0x4f, 0xe4, 0x44, 0x44, 0x80, 0x00, 0x00, 0x07, 0x84, 0x44, 0x44, 0x47, 0x70, 0x00, 0x00, 0x00, 0x07, 0x88, 0x87, 0x70, 0x00, 0x00, 0xf8, 0xff, 0x00, 0x7f, 0x00, 0x1f, 0x00, 0x07]
        )
        {
            Priority = 99,
            Indicators = 
            [
                MatchIndicator.ClickNCreate
            ]
        },
        new
        (
            alias: "MMF15_ICON",
            data: [0x00, 0x00, 0x01, 0x11, 0x11, 0x10, 0x00, 0x00, 0x00, 0x00, 0x11, 0x99, 0x99, 0x91, 0x00, 0x00, 0x00, 0x01, 0x90, 0x01, 0x99, 0x99, 0x10, 0x00, 0x00, 0x19, 0x99, 0x0b, 0x19, 0x99, 0x91, 0x00, 0x01, 0x99, 0x99, 0x90, 0xb1, 0x99, 0x99, 0x10, 0x01, 0x99, 0x91, 0x00, 0xbb, 0x19, 0x99, 0x10, 0x01, 0x99, 0x99, 0x1b, 0xb1, 0x19, 0x99, 0x10, 0x01, 0x99, 0x90, 0x0b, 0x11, 0x99, 0x99, 0x10, 0x01, 0x99, 0x90, 0xbb, 0xb1, 0x99, 0x99, 0x10, 0x01, 0x99, 0x99, 0x0b, 0xb1, 0x99, 0x99, 0x10, 0x00, 0x19, 0x99, 0x90, 0x11, 0x19, 0x91, 0x00, 0x00, 0x01, 0x99, 0x99, 0x99, 0x99, 0x10, 0x00, 0x00, 0x00, 0x19, 0x99, 0x99, 0x91, 0x00, 0x00, 0x00, 0x00, 0x01, 0x11, 0x11, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00, 0xf8, 0x1f, 0x00, 0x00]
        )
        {
            Priority = 99,
            Indicators = 
            [
                MatchIndicator.MultimediaFusion20
            ],
        },
        new
        (
            alias: "MAGIC_WORD_MMF2",
            data: "MMF2",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".mfa"],
            Indicators = 
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_CnC2_MF2",
            data: "CnC2",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".mfa"],
            Indicators = 
            [
                MatchIndicator.MultimediaFusion, 
                MatchIndicator.MultimediaFusion20
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_MFU2",
            data: "MFU2",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".mfa"],
            Indicators =
            [
                MatchIndicator.MultimediaFusion,
                MatchIndicator.MultimediaFusion20
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAME_VTZ",
            data: "PAME",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".ccn", ".hts"],
            Indicators = 
            [
                MatchIndicator.Vitalize
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAPP_VTZ",
            data: "PAPP",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".ccn", ".hts"],
            Indicators =
            [
                MatchIndicator.Vitalize
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAMU_VTZ",
            data: "PAMU",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".ccn", ".hts"],
            Indicators =
            [
                MatchIndicator.Vitalize
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_VTZ",
            data: "VTZ ",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".ccn", ".hts"],
            Indicators =
            [
                MatchIndicator.Vitalize
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_GAME_CNC",
            data: "GAME",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".cca"],
            Indicators =
            [
                MatchIndicator.ClickNCreate
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAME_CNC",
            data: "PAME",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".cca"],
            Indicators =
            [
                MatchIndicator.ClickNCreate
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_GAMU",
            data: "GAMU",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".cca"],
            Indicators =
            [
                MatchIndicator.ClickNCreate
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAMU_CNC",
            data: "PAMU",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".cca"],
            Indicators =
            [
                MatchIndicator.ClickNCreate
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_CnC2_CNC",
            data: "CnC2",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".cca"],
            Indicators =
            [
                MatchIndicator.ClickNCreate
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_GAME_TGF",
            data: "GAME",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".gam"],
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAME_TGF",
            data: "PAME",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".gam"],
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_GAPP",
            data: "GAPP",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".gam"],
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ],
            Position = 0,
        },
        new
        (
            alias: "MAGIC_WORD_PAPP_TGF",
            data: "PAPP",
            isUnicode: false
        )
        {
            Priority = 1,
            FileExtensions = [".gam"],
            Indicators = 
            [
                MatchIndicator.TheGamesFactory
            ],
            Position = 0,
        },
    ];
}
