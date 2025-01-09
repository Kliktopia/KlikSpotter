# KlikSpotter
Unofficial tool to find software made using Klik &amp; Play, The Games Factory, Click &amp; Create, Multimedia Fusion and Clickteam Fusion 2.5

Example KlikSpotter.config
```
{
    "fileExtensions": [
        {
            "extension": ".7z",
            "isArchive": true
        },
        {
            "extension": ".bz2",
            "isArchive": true
        },
        {
            "extension": ".cbr",
            "isArchive": true
        },
        {
            "extension": ".cbz",
            "isArchive": true
        },
        {
            "extension": ".cca",
            "isArchive": false,
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "extension": ".ccn",
            "isArchive": false,
            "indicators": [
                "Vitalize"
            ]
        },
        {
            "extension": ".exe"
        },
        {
            "extension": ".gam",
            "isArchive": false,
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "extension": ".gz",
            "isArchive": true
        },
        {
            "extension": ".hts",
            "isArchive": false,
            "indicators": [
                "Vitalize"
            ]
        },
        {
            "extension": ".mfa",
            "isArchive": false,
            "indicators": [
                "MultimediaFusion"
            ]
        },
        {
            "extension": ".r00",
            "isArchive": true
        },
        {
            "extension": ".rar",
            "isArchive": true
        },
        {
            "extension": ".tar",
            "isArchive": true
        },
        {
            "extension": ".taz",
            "isArchive": true
        },
        {
            "extension": ".tb2",
            "isArchive": true
        },
        {
            "extension": ".tbz",
            "isArchive": true
        },
        {
            "extension": ".tbz2",
            "isArchive": true
        },
        {
            "extension": ".tgz",
            "isArchive": true
        },
        {
            "extension": ".tz2",
            "isArchive": true
        },
        {
            "extension": ".zip",
            "isArchive": true
        },
        {
            "extension": ".zipx",
            "isArchive": true
        }
    ],
    "filesThatIndicatesMatch": [
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
        "rubberovine.dll"
    ],
    "filesToRemove": [
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
        "uninstall.exe"
    ],
    "searchPatterns": [
        {
            "alias": "MAGIC_WORD_CNC2_CNC",
            "data": ["CNC2"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".cca"
            ],
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "MAGIC_WORD_CNC2_MF2",
            "data": ["CNC2"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".mfa"
            ],
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "MAGIC_WORD_GAME_CNC",
            "data": ["GAME"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".cca"
            ],
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "MAGIC_WORD_GAME_TGF",
            "data": ["GAME"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".gam"
            ],
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MAGIC_WORD_GAMU",
            "data": ["GAMU"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".cca"
            ],
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "MAGIC_WORD_GAPP",
            "data": ["GAPP"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".gam"
            ],
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MAGIC_WORD_MFU2",
            "data": ["MFU2"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".mfa"
            ],
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "MAGIC_WORD_MMF2",
            "data": ["MMF2"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".mfa"
            ],
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAME_CNC",
            "data": ["PAME"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".cca"
            ],
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAME_TGF",
            "data": ["PAME"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".gam"
            ],
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAME_VTZ",
            "data": ["PAME"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".ccn",
                ".hts"
            ],
            "indicators": [
                "Vitalize"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAMU_CNC",
            "data": ["PAMU"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".cca"
            ],
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAMU_VTZ",
            "data": ["PAMU"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".ccn",
                ".hts"
            ],
            "indicators": [
                "Vitalize"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAPP_TGF",
            "data": ["PAPP"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".gam"
            ],
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MAGIC_WORD_PAPP_VTZ",
            "data": ["PAPP"],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".ccn",
                ".hts"
            ],
            "indicators": [
                "Vitalize"
            ]
        },
        {
            "alias": "MAGIC_WORD_VTZ",
            "data": ["VTZ "],
            "priority": 1,
            "position": 0,
            "fileExtensions": [
                ".ccn",
                ".hts"
            ],
            "indicators": [
                "Vitalize"
            ]
        },
        {
            "alias": "1998 IMSI (Unicode)",
            "data": ["1", 0, "9", 0, "9", 0, "8", 0, " ", 0, "I", 0, "M", 0, "S", 0, "I", 0],
            "priority": 5
        },
        {
            "alias": "CNCRT32.EXE (Unicode)",
            "data": ["C", 0, "N", 0, "C", 0, "R", 0, "T", 0, "3", 0, "2", 0, ".", 0, "E", 0, "X", 0, "E", 0],
            "priority": 5
        },
        {
            "alias": "CNC_INSTALLER",
            "data": ["C\u0026C Stand Alone App Installer"],
            "priority": 5,
            "indicators": [
                "Installer",
                "ClickNCreate",
                "ClickNCreateInstaller"
            ]
        },
        {
            "alias": "CNCRT32.EXE",
            "data": ["CNCRT32.EXE"],
            "priority": 5
        },
        {
            "alias": "CNCSA.BIN",
            "data": ["CNCSA.BIN"],
            "priority": 5,
            "indicators": [
                "ClickNCreate",
                "MultimediaFusion"
            ]
        },
        {
            "alias": "CNCSA32.EXE",
            "data": ["CNCSA32.EXE"],
            "priority": 5,
            "indicators": [
                "ClickNCreate",
                "MultimediaFusion"
            ]
        },
        {
            "alias": "CncEditClass",
            "data": ["CncEditClass"],
            "priority": 5,
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "TGF_INSTALLER",
            "data": ["GFSAS", 0, 0],
            "priority": 5,
            "indicators": [
                "Installer",
                "TheGamesFactory",
                "TheGamesFactoryInstaller"
            ]
        },
        {
            "alias": "TGF_INSTALLER_ALT",
            "data": ["GFSAS32.EXE"],
            "priority": 5,
            "indicators": [
                "Installer",
                "TheGamesFactory",
                "TheGamesFactoryInstaller"
            ]
        },
        {
            "alias": "Klik \u0026 Play",
            "data": ["Klik \u0026 Play"],
            "priority": 5,
            "indicators": [
                "KlikNPlay"
            ]
        },
        {
            "alias": "KNP_INSTALLER",
            "data": ["Klik \u0026 Play stand alone installer"],
            "priority": 5,
            "indicators": [
                "KlikNPlay",
                "KlikNPlayInstaller"
            ]
        },
        {
            "alias": "MMFS2.dll",
            "data": ["MMFS2.dll"],
            "priority": 5,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "lnchrt.exe (Unicode)",
            "data": ["l", 0, "n", 0, "c", 0, "h", 0, "r", 0, "t", 0, ".", 0, "e", 0, "x", 0, "e", 0],
            "priority": 5
        },
        {
            "alias": "mmf2d3d11.dll (Unicode)",
            "data": ["m", 0, "m", 0, "f", 0, "2", 0, "d", 0, "3", 0, "d", 0, "1", 0, "1", 0, ".", 0, "d", 0, "l", 0, "l", 0],
            "priority": 5,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "mmfs2.dll (Unicode)",
            "data": ["m", 0, "m", 0, "f", 0, "s", 0, "2", 0, ".", 0, "d", 0, "l", 0, "l", 0],
            "priority": 5,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "mmfs2.dll",
            "data": ["mmfs2.dll"],
            "priority": 5,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "INSTALL_MAKER",
            "data": ["wwgT)H", 0, 0, 0, 0],
            "priority": 5,
            "indicators": [
                "Installer",
                "InstallMakerInstaller"
            ]
        },
        {
            "alias": "INSTALL_CREATOR",
            "data": ["wwgT)H"],
            "priority": 5,
            "indicators": [
                "Installer",
                "InstallCreatorInstaller"
            ]
        },
        {
            "alias": "CNCS232",
            "data": ["CNCS232"],
            "priority": 10,
            "indicators": [
                "ClickNCreate",
                "MultimediaFusion"
            ]
        },
        {
            "alias": "Cc2EditClass",
            "data": ["Cc2EditClass"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion"
            ]
        },
        {
            "alias": "Cc2MainClass",
            "data": ["Cc2MainClass"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion"
            ]
        },
        {
            "alias": "Click \u0026 Create",
            "data": ["Click \u0026 Create"],
            "priority": 10,
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "CncMainClass",
            "data": ["CncMainClass"],
            "priority": 10,
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "GFactory",
            "data": ["GFactory"],
            "priority": 10,
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MMFUSION",
            "data": ["MMFUSION"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion"
            ]
        },
        {
            "alias": "Multimedia Fusion Express",
            "data": ["Multimedia Fusion Express"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusionExpress"
            ]
        },
        {
            "alias": "cncrt32.exe (Unicode)",
            "data": ["c", 0, "n", 0, "c", 0, "r", 0, "t", 0, "3", 0, "2", 0, ".", 0, "e", 0, "x", 0, "e", 0],
            "priority": 10
        },
        {
            "alias": "cncrt32.exe",
            "data": ["cncrt32.exe"],
            "priority": 10
        },
        {
            "alias": "cncsa.bin",
            "data": ["cncsa.bin"],
            "priority": 10
        },
        {
            "alias": "cncsa32.EXE",
            "data": ["cncsa32.EXE"],
            "priority": 10,
            "indicators": [
                "ClickNCreate",
                "MultimediaFusion"
            ]
        },
        {
            "alias": "mmf2d3d8.dll (Unicode)",
            "data": ["m", 0, "m", 0, "f", 0, "2", 0, "d", 0, "3", 0, "d", 0, "8", 0, ".", 0, "d", 0, "l", 0, "l", 0],
            "priority": 10,
            "indicators": [
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "mmf2d3d9.dll (Unicode)",
            "data": ["m", 0, "m", 0, "f", 0, "2", 0, "d", 0, "3", 0, "d", 0, "9", 0, ".", 0, "d", 0, "l", 0, "l", 0],
            "priority": 10,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "mmf2d3d11.dll",
            "data": ["mmf2d3d11.dll"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "mmf2d3d8.dll",
            "data": ["mmf2d3d8.dll"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion"
            ]
        },
        {
            "alias": "mmf2d3d9.dll",
            "data": ["mmf2d3d9.dll"],
            "priority": 10,
            "indicators": [
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "\u003Cdescription\u003EApplication\u003C/description\u003E (Unicode)",
            "data": ["\u003C", 0, "d", 0, "e", 0, "s", 0, "c", 0, "r", 0, "i", 0, "p", 0, "t", 0, "i", 0, "o", 0, "n", 0, "\u003E", 0, "A", 0, "p", 0, "p", 0, "l", 0, "i", 0, "c", 0, "a", 0, "t", 0, "i", 0, "o", 0, "n", 0, "\u003C", 0, "/", 0, "d", 0, "e", 0, "s", 0, "c", 0, "r", 0, "i", 0, "p", 0, "t", 0, "i", 0, "o", 0, "n", 0, "\u003E", 0],
            "priority": 20
        },
        {
            "alias": "Cannot create subdirectory in temporary directory ! (Unicode)",
            "data": ["C", 0, "a", 0, "n", 0, "n", 0, "o", 0, "t", 0, " ", 0, "c", 0, "r", 0, "e", 0, "a", 0, "t", 0, "e", 0, " ", 0, "s", 0, "u", 0, "b", 0, "d", 0, "i", 0, "r", 0, "e", 0, "c", 0, "t", 0, "o", 0, "r", 0, "y", 0, " ", 0, "i", 0, "n", 0, " ", 0, "t", 0, "e", 0, "m", 0, "p", 0, "o", 0, "r", 0, "a", 0, "r", 0, "y", 0, " ", 0, "d", 0, "i", 0, "r", 0, "e", 0, "c", 0, "t", 0, "o", 0, "r", 0, "y", 0, " ", 0, "!", 0],
            "priority": 20
        },
        {
            "alias": "Invalid data in executable file ! (Unicode)",
            "data": ["I", 0, "n", 0, "v", 0, "a", 0, "l", 0, "i", 0, "d", 0, " ", 0, "d", 0, "a", 0, "t", 0, "a", 0, " ", 0, "i", 0, "n", 0, " ", 0, "e", 0, "x", 0, "e", 0, "c", 0, "u", 0, "t", 0, "a", 0, "b", 0, "l", 0, "e", 0, " ", 0, "f", 0, "i", 0, "l", 0, "e", 0, " ", 0, "!", 0],
            "priority": 20
        },
        {
            "alias": "Multimedia Fusion Application Runtime (Unicode)",
            "data": ["M", 0, "u", 0, "l", 0, "t", 0, "i", 0, "m", 0, "e", 0, "d", 0, "i", 0, "a", 0, " ", 0, "F", 0, "u", 0, "s", 0, "i", 0, "o", 0, "n", 0, " ", 0, "A", 0, "p", 0, "p", 0, "l", 0, "i", 0, "c", 0, "a", 0, "t", 0, "i", 0, "o", 0, "n", 0, " ", 0, "R", 0, "u", 0, "n", 0, "t", 0, "i", 0, "m", 0, "e", 0],
            "priority": 20,
            "indicators": [
                "MultimediaFusion"
            ]
        },
        {
            "alias": "The Games Factory Application Runtime (Unicode)",
            "data": ["T", 0, "h", 0, "e", 0, " ", 0, "G", 0, "a", 0, "m", 0, "e", 0, "s", 0, " ", 0, "F", 0, "a", 0, "c", 0, "t", 0, "o", 0, "r", 0, "y", 0, " ", 0, "A", 0, "p", 0, "p", 0, "l", 0, "i", 0, "c", 0, "a", 0, "t", 0, "i", 0, "o", 0, "n", 0, " ", 0, "R", 0, "u", 0, "n", 0, "t", 0, "i", 0, "m", 0, "e", 0],
            "priority": 20,
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MAGIC",
            "data": [192, 0, 20, 144, 0, 0, 0, 0, 1, 0, 6, 0, 15, 0, 152, 0, 22, 0, 0, 0, 0, 0],
            "priority": 30
        },
        {
            "alias": "MAGIC_ALT",
            "data": [192, 8, 20, 144, 0, 0, 0, 0, 1, 0, 6, 0, 15, 0, 152, 0, 22, 0, 0, 0, 0, 0],
            "priority": 30
        },
        {
            "alias": "KNP_ICON",
            "data": [0, 0, 0, 0, 0, 0, 4, "DDDDD", 0, 0, 0, 0, 0, 0, 0, 0, 0, "DL", 24, 24, 24, "D@", 0, 0, 0, 0, 0, 0, 0, 0, "D", 24, 24, 24, 24, 24, "D", 11, 176, 0, 0, 0, 0, 0, 0, 4, 24, 24, 24, 24, 24, 24, "D", 11, 176, 0, 0, 0, 0, 0, 0, "L", 24, 24, 24, 198, 24, 24, "D", 11, 176, "@", 0, 0, 0, 0, 4, 24, 24, 198, "fff", 24, "D", 11, 176, 0, 0, 0, 0, 0, "L", 24, 24, "fffl", 24, "D", 11, 187, 187, 187, 0, 0, 4, 24, 24, 198, "fl", 24, 24, 24, "D", 11, 176, 0, 11, 176, 0, 4, 24, 24, "ff", 24, 24, 24, 24, "D", 11, 176, "d", 11, 176, 0, "L", 24, 24, "f", 24, 24, "DDLD", 11, 176, "d", 11, 176, 0, "L", 24, 198, "o", 24, 196, 0, 0, 0, "d", 11, 176, 0, 11, 176, 0, "L", 24, "fl", 24, "@", 187, 187, 187, 12, 11, 187, 187, 187, 0, 4, 24, 198, "f", 252, 196, 11, 176, 11, 176, "L", 192, 0, 0, 0, 196, 4, 24, 198, "f", 24, 196, 11, 176, 11, 187, 12, 24, 24, 24, 24, 196, 4, 24, 198, "o", 24, 196, 11, 176, 187, 11, 12, 24, 27, "l", 24, 196, 4, 24, 198, "o", 24, 24, "@", 187, 176, "@", 0, 24, "of", 24, 196, 4, "DFdD", 24, 196, 11, 187, 12, 24, 198, "of", 24, 196, 4, "DDdDL", 192, 187, 11, 176, 24, "fof", 24, 196, 4, "D", 0, "DD", 0, 192, 187, 11, 176, 24, "fof", 24, 196, 4, "@", 187, 4, "@", 187, 0, 187, 11, 176, 24, "f", 246, "l", 24, "@", 4, "@", 187, 4, 11, 176, 24, 11, 187, 12, 198, "f", 246, "l", 24, "@", 4, "@", 187, 0, 187, 12, 24, 192, 0, 24, "fof", 24, 24, "@", 4, "@", 187, 11, 176, 24, 24, 24, 24, "ffof", 24, 196, 0, 4, "@", 187, 187, 12, 24, 24, "fffo", 246, "l", 24, 196, 0, 4, "@", 187, 176, 24, 24, 27, "ffo", 246, "l", 24, 24, "@", 0, 4, "@", 187, 187, 12, 24, "f", 255, 255, 246, "f", 24, 24, 196, 0, 0, 4, "@", 187, 11, 176, 198, "ffffl", 24, 24, "@", 0, 0, 4, "@", 187, 0, 187, 12, "fff", 24, 24, 24, 196, 0, 0, 0, 0, "@", 187, 0, 11, 176, 24, 24, 24, 24, 24, 196, "@", 0, 0, 0, 0, 0, 0, 0, 0, 4, "L", 24, 24, 24, "D@", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, "DDD", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            "priority": 99,
            "indicators": [
                "KlikNPlay"
            ]
        },
        {
            "alias": "MMF15_ICON",
            "data": [0, 0, 1, 17, 17, 16, 0, 0, 0, 0, 17, 153, 153, 145, 0, 0, 0, 1, 144, 1, 153, 153, 16, 0, 0, 25, 153, 11, 25, 153, 145, 0, 1, 153, 153, 144, 177, 153, 153, 16, 1, 153, 145, 0, 187, 25, 153, 16, 1, 153, 153, 27, 177, 25, 153, 16, 1, 153, 144, 11, 17, 153, 153, 16, 1, 153, 144, 187, 177, 153, 153, 16, 1, 153, 153, 11, 177, 153, 153, 16, 0, 25, 153, 144, 17, 25, 145, 0, 0, 1, 153, 153, 153, 153, 16, 0, 0, 0, 25, 153, 153, 145, 0, 0, 0, 0, 1, 17, 17, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 0, 0, 248, 31, 0, 0],
            "priority": 99,
            "indicators": [
                "MultimediaFusion20"
            ]
        },
        {
            "alias": "CNC_ICON",
            "data": [0, 0, 135, 136, 135, "x", 128, 0, 0, 7, 132, "DDGp", 0, 0, "tDDDD", 136, 128, 7, 132, "DDDDGp", 4, "DDDD34", 128, 3, "343K4C", 136, "s4", 187, ";;4DHs4D", 179, ";4DHs4D", 179, ";4CH", 3, "4", 180, "KD", 179, "4", 128, 11, "34DDDD", 128, 7, 132, "L", 196, "DDGp", 0, "tO", 228, "DD", 128, 0, 0, 7, 132, "DDGp", 0, 0, 0, 7, 136, 135, "p", 0, 0, 248, 255, 0, 127, 0, 31, 0, 7],
            "priority": 99,
            "indicators": [
                "ClickNCreate"
            ]
        },
        {
            "alias": "TGF_ICON",
            "data": [0, "DN", 26, 238, 228, "L@", 4, 24, 239, 255, 255, 254, 236, 0, "N", 27, 244, "Dn", 255, 252, "D", 26, 239, 252, 196, "N", 254, 254, 224, "N", 254, 244, "DDN", 255, 236, 15, 254, 24, 196, "DN", 255, 240, 15, 254, 198, 244, "DN", 255, 254, 15, 254, 198, 244, "DFo", 244, "O", 254, 198, "dDDo", 244, "N", 254, 244, "D", 244, "N", 255, 228, "N", 238, 254, 254, 244, "Fn", 224, "L", 239, 255, 255, 252, "Dn@", 14, 238, 255, 254, 254, 255, 238, 192, 4, "L", 238, 255, 255, 238, 228, 192, 0, 12, 196, 24, 24, 192, "D@", 143, 247, 0, 7, 0, 15, 0, 0],
            "priority": 99,
            "indicators": [
                "TheGamesFactory"
            ]
        },
        {
            "alias": "MMFEXPRESS_ICON",
            "data": ["(", 0, 0, 0, 16, 0, 0, 0, " ", 0, 0, 0, 1, 0, 4, 0, 0, 0, 0, 0, 128, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 0, 0, 128, 0, 0, 0, 128, 128, 0, 128, 0, 0, 0, 128, 0, 128, 0, 128, 128, 0, 0, 192, 192, 192, 0, 128, 128, 128, 0, 0, 0, 255, 0, 0, 255, 0, 0, 0, 255, 255, 0, 255, 0, 0, 0, 255, 0, 255, 0, 255, 255, 0, 0, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, "x", 0, 135, "p", 0, 0, 7, 128, "DD", 0, 8, "p", 0, "tDDDD@", 7, 7, "DDDDDD", 0, 4, "DD@DDD@", 4, "DD@", 244, "DD@DDD@", 255, "DDD", 4, "DD@", 247, "DD@", 4, "DD@tDDG", 0, "DD@DDDp", 0, 4, "DDDD@", 0, 0, 0, 0, "DD", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, "xD", 255, 255, "DD", 248, 7, "DD", 224, 1, 8, "p", 192, 0, "DD", 128, 0, "DD", 128, 0, "DD", 128, 0, 0, 135, 0, 0, "DD", 128, 0, "HD", 128, 0, "DD", 192, 1, 0, 8, 224, 7, "DD", 252, "?@", 132, 255, 255, "DD", 255, 255, "@", 0],
            "priority": 99,
            "indicators": [
                "MultimediaFusion",
                "MultimediaFusionExpress"
            ]
        }
    ],
    "outputDirectories": [
        {
            "type": "Found",
            "path": "!FOUND"
        },
        {
            "type": "Failed",
            "path": "!PROBLEMATIC"
        },
        {
            "type": "Moved",
            "path": "!MOVED"
        },
        {
            "type": "Indeterminate",
            "path": "!UNKNOWN"
        },
        {
            "type": "KlikNPlayInstaller",
            "path": "!KNP_INSTALLERS"
        },
        {
            "type": "ClickNCreateInstaller",
            "path": "!CNC_INSTALLERS"
        },
        {
            "type": "TheGamesFactoryInstaller",
            "path": "!TGF_INSTALLERS"
        },
        {
            "type": "InstallCreatorInstaller",
            "path": "!IC_INSTALLERS"
        },
        {
            "type": "InstallMakerInstaller",
            "path": "!IM_INSTALLERS"
        }
    ]
}
```

