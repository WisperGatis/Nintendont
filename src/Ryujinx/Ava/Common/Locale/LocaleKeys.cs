namespace Ryujinx.Ava.Common.Locale
{
    /// <summary>
    /// Enum containing all localization keys from the en_US.json file.
    /// This is a comprehensive implementation to fix build errors.
    /// </summary>
    internal enum LocaleKeys
    {
        // Menu Bar Items
        MenuBarFile,
        MenuBarFileOpenFromFile,
        MenuBarFileOpenFromFileError,
        MenuBarFileOpenUnpacked,
        MenuBarFileOpenEmuFolder,
        MenuBarFileOpenLogsFolder,
        MenuBarFileExit,
        MenuBarFileOpenApplet,
        MenuBarFileOpenAppletOpenMiiAppletToolTip,
        LoadApplicationFileTooltip,
        LoadApplicationFolderTooltip,
        OpenRyujinxFolderTooltip,
        OpenRyujinxLogsTooltip,
        ExitTooltip,
        
        // Options Menu
        MenuBarOptions,
        MenuBarOptionsToggleFullscreen,
        MenuBarOptionsStartGamesInFullscreen,
        MenuBarOptionsStopEmulation,
        MenuBarOptionsSettings,
        MenuBarOptionsManageUserProfiles,
        MenuBarOptionsShowConsole,
        MenuBarOptionsChangeLanguage,
        MenuBarShowFileTypes,
        OpenSettingsTooltip,
        OpenProfileManagerTooltip,
        MenuBarOptionsPauseEmulation,
        MenuBarOptionsResumeEmulation,
        StopEmulationTooltip,
        
        // Actions Menu
        MenuBarActions,
        MenuBarOptionsSimulateWakeUpMessage,
        MenuBarActionsScanAmiibo,
        
        // Tools Menu
        MenuBarTools,
        MenuBarToolsInstallFirmware,
        MenuBarFileToolsInstallFirmwareFromFile,
        MenuBarFileToolsInstallFirmwareFromDirectory,
        MenuBarToolsManageFileTypes,
        MenuBarFileToolsTakeScreenshot,
        MenuBarFileToolsHideUi,
        MenuBarToolsInstallFileTypes,
        MenuBarToolsUninstallFileTypes,
        
        // View Menu
        MenuBarView,
        MenuBarViewWindow,
        MenuBarViewWindow720,
        MenuBarViewWindow1080,
        
        // Help Menu
        MenuBarHelp,
        MenuBarHelpCheckForUpdates,
        CheckUpdatesTooltip,
        MenuBarHelpAbout,
        OpenAboutTooltip,

        // Settings Tab Items
        SettingsTabInputDirectMouseAccess,
        SettingsTabSystemMemoryManagerMode,
        SettingsTabSystemMemoryManagerModeSoftware,
        SettingsTabSystemMemoryManagerModeHost,
        SettingsTabSystemMemoryManagerModeHostUnchecked,
        SettingsTabSystemUseHypervisor,
        
        // Audio Settings
        SettingsTabSystemAudioBackend,
        AudioBackendTooltip,
        SettingsTabSystemAudioBackendDummy,
        SettingsTabSystemAudioBackendOpenAL,
        SettingsTabSystemAudioBackendSoundIO,
        SettingsTabSystemAudioBackendSDL2,
        SettingsTabSystemAudioVolume,
        
        // Graphics Settings
        SettingsTabGraphicsAspectRatio,
        SettingsTabGraphicsAspectRatio4x3,
        SettingsTabGraphicsAspectRatio16x9,
        SettingsTabGraphicsAspectRatio16x10,
        SettingsTabGraphicsAspectRatio21x9,
        SettingsTabGraphicsAspectRatio32x9,
        SettingsTabGraphicsAspectRatioStretch,
        
        // Common Dialog Buttons
        Generic_Ok,
        Generic_Cancel,
        Generic_Close,
        Generic_Save,
        Generic_Error,
        Generic_Warning,
        Generic_Success,
        Generic_Info,
        
        // Common UI Elements
        IconSize,
        IconSizeTooltip,
        CommonShowNames,
        MenuSearch,
        OrderAscending,
        OrderDescending,
        CommonSort,
        
        // Input Dialog Buttons
        InputDialogOk,
        InputDialogYes,
        InputDialogNo,
        InputDialogCancel,
        
        // Application Error Dialog
        DialogMessageAppExit,
        DialogMessageAppExitMessage,
        DialogMessageAppExitSubMessage,
        DialogMessageAppExitInputBoxMessage,
        
        // Common Error Messages
        ErrorWindowTitle,
        ErrorMessageFatal,
        ErrorMessageSoundBackend,
        ErrorMessageGpu,
        ErrorMessageAudio,
        ErrorMessageSystemFiles,
        ErrorMessageSystemShaders,
        
        // Dialog Titles
        RyujinxInfo,
        RyujinxConfirm,
        RyujinxUpdater,
        DialogWarning,
        DialogErrorTitle,
        DialogUserErrorDialogTitle,
        DialogControllerAppletTitle,
        DialogUpdaterTitle,
        
        // Status Bar
        StatusBarGamesLoaded,
        StatusBarSystemVersion,
        StatusBarPlayControlsNotRunning,
        StatusBarPlayControlsRunning,
        StatusBarPlayControlsPaused,
        AspectRatioTooltip,
        AudioVolumeTooltip,
        
        // Loading Messages
        LoadingHeading,
        CompilingPPTC,
        CompilingShaders,
        PackagingShaders,
        
        // File Dialog
        FileDialogAllTypes,
        OpenFileDialogTitle,
        OpenFolderDialogTitle,
        AllSupportedFormats,
        
        // Game Dialogs
        DialogLoadAppGameAlreadyLoadedMessage,
        DialogLoadAppGameAlreadyLoadedSubMessage,
        DialogPerformanceCheckLoggingEnabledMessage,
        DialogPerformanceCheckLoggingEnabledConfirmMessage,
        DialogPerformanceCheckShaderDumpEnabledMessage,
        DialogPerformanceCheckShaderDumpEnabledConfirmMessage,
        DialogPPTCDeletionMessage,
        DialogShaderDeletionMessage,
        DialogModManagerDeletionWarningMessage,
        DialogModManagerDeletionAllWarningMessage,
        DialogThemeRestartMessage,
        DialogThemeRestartSubMessage,
        DialogRestartRequiredMessage,
        DialogUpdaterConvertFailedMessage,
        DialogUpdaterCancelUpdateMessage,
        DialogUpdaterAlreadyOnLatestVersionMessage,
        DialogUpdaterFailedToGetVersionMessage,
        DialogUpdaterConvertFailedGithubMessage,
        DialogUpdaterCompleteMessage,
        DialogUpdaterRestartMessage,
        DialogInstallFileTypesSuccessMessage,
        DialogUninstallFileTypesSuccessMessage,
        DialogInstallFileTypesErrorMessage,
        DialogUninstallFileTypesErrorMessage,
        DialogUpdateAddUpdateErrorMessage,
        DialogLoadFileErrorMessage,
        DialogMessageCreateSaveErrorMessage,
        DialogMessageFindSaveErrorMessage,
        DialogOpenSettingsWindowLabel,
        DialogMessageDialogErrorExceptionMessage,
        DialogSoftwareKeyboardErrorExceptionMessage,
        DialogUserProfileDeletionWarningMessage,
        DialogUserProfileDeletionConfirmMessage,
        
        // User Profile Dialogs
        UserProfileWindowTitle,
        UserEditorTitleCreate,
        UserEditorTitle,
        UserProfileEmptyNameError,
        UserProfileUnsavedChangesTitle,
        UserProfileUnsavedChangesMessage,
        UserProfileUnsavedChangesSubMessage,
        
        // Titles and Headings
        TitleBundledDlcLabel,
        TitleBundledUpdateVersionLabel,
        TitleUpdateVersionLabel,
        GameUpdateWindowHeading,
        SaveManagerHeading,
        AboutPageDeveloperListMore,
        ProfileImageSelectionHeader,
        UserProfilesRecoverHeading,
        CheatWindowTitle,
        CheatWindowHeading,
        DlcWindowTitle,
        ModWindowTitle,
        Settings,
        
        // Keyboard Keys
        KeyUnknown,
        KeyShiftLeft,
        KeyShiftRight,
        KeyControlLeft,
        KeyControlRight,
        KeyAltLeft,
        KeyAltRight,
        KeyWinLeft,
        KeyWinRight,
        KeyUp,
        KeyDown,
        KeyLeft,
        KeyRight,
        KeyEnter,
        KeyEscape,
        KeySpace,
        KeyTab,
        KeyBackSpace,
        KeyInsert,
        KeyDelete,
        KeyPageUp,
        KeyPageDown,
        KeyHome,
        KeyEnd,
        KeyCapsLock,
        KeyScrollLock,
        KeyPrintScreen,
        KeyPause,
        KeyNumLock,
        KeyClear,
        KeyKeypad0,
        KeyKeypad1,
        KeyKeypad2,
        KeyKeypad3,
        KeyKeypad4,
        KeyKeypad5,
        KeyKeypad6,
        KeyKeypad7,
        KeyKeypad8,
        KeyKeypad9,
        KeyKeypadDivide,
        KeyKeypadMultiply,
        KeyKeypadSubtract,
        KeyKeypadAdd,
        KeyKeypadDecimal,
        KeyKeypadEnter,
        KeyNumber0,
        KeyNumber1,
        KeyNumber2,
        KeyNumber3,
        KeyNumber4,
        KeyNumber5,
        KeyNumber6,
        KeyNumber7,
        KeyNumber8,
        KeyNumber9,
        KeyTilde,
        KeyGrave,
        KeyMinus,
        KeyPlus,
        KeyBracketLeft,
        KeyBracketRight,
        KeySemicolon,
        KeyQuote,
        KeyComma,
        KeyPeriod,
        KeySlash,
        KeyBackSlash,
        KeyUnbound,
        
        // Mac-specific key mappings
        KeyMacControlLeft,
        KeyMacControlRight,
        KeyMacAltLeft,
        KeyMacAltRight,
        KeyMacWinLeft,
        KeyMacWinRight,
        
        // Gamepad Input IDs
        GamepadLeftStick,
        GamepadRightStick,
        GamepadLeftShoulder,
        GamepadRightShoulder,
        GamepadLeftTrigger,
        GamepadRightTrigger,
        GamepadDpadUp,
        GamepadDpadDown,
        GamepadDpadLeft,
        GamepadDpadRight,
        GamepadMinus,
        GamepadPlus,
        GamepadGuide,
        GamepadMisc1,
        GamepadPaddle1,
        GamepadPaddle2,
        GamepadPaddle3,
        GamepadPaddle4,
        GamepadTouchpad,
        GamepadSingleLeftTrigger0,
        GamepadSingleRightTrigger0,
        GamepadSingleLeftTrigger1,
        GamepadSingleRightTrigger1,
        
        // Stick Input IDs
        StickLeft,
        StickRight,
        
        // Volume and Game
        VolumeShort,
        Game,
        
        // Controller Settings
        ControllerSettingsDeviceDisabled,
        ControllerSettingsProfileDefault,
        ControllerSettingsTriggerZL,
        ControllerSettingsTriggerZR,
        ControllerSettingsTriggerL,
        ControllerSettingsTriggerR,
        ControllerSettingsButtonMinus,
        ControllerSettingsButtonPlus,
        ControllerSettingsLStick,
        ControllerSettingsRStick,
        ControllerSettingsDpad,
        ControllerSettingsA,
        ControllerSettingsB,
        ControllerSettingsX,
        ControllerSettingsY,
        ControllerSettingsL,
        ControllerSettingsR,
        ControllerSettingsZL,
        ControllerSettingsZR,
        ControllerSettingsPlus,
        ControllerSettingsMinus,
        ControllerSettingsCapture,
        ControllerSettingsHome,
        ControllerSettingsStickButton,
        ControllerSettingsDpadButton,
        ControllerSettingsLeftShoulderButton,
        ControllerSettingsRightShoulderButton,
        ControllerSettingsLeftTriggerButton,
        ControllerSettingsRightTriggerButton,
        
        // Controller Stick Settings
        ControllerSettingsStickStick,
        ControllerSettingsStickDeadzone,
        ControllerSettingsStickSensitivity,
        ControllerSettingsStickInvert,
        ControllerSettingsStickRange,
        ControllerSettingsStickUp,
        ControllerSettingsStickDown,
        ControllerSettingsStickLeft,
        ControllerSettingsStickRight,
        
        // Stick Inversion Settings
        ControllerSettingsStickInvertXAxis,
        ControllerSettingsStickInvertYAxis,
        ControllerSettingsStickInvertBothAxes,
        
        // Stick Rotation Settings
        ControllerSettingsRotate90,
        ControllerSettingsRotate180,
        ControllerSettingsRotate270,
        ControllerSettingsStickRotate90,
        ControllerSettingsStickRotate180,
        ControllerSettingsStickRotate270,
        
        // D-pad Settings
        ControllerSettingsDPad,
        ControllerSettingsDPadUp,
        ControllerSettingsDPadDown,
        ControllerSettingsDPadLeft,
        ControllerSettingsDPadRight,
        ControllerSettingsDPadButton,
        
        // Trigger Settings
        ControllerSettingsTriggerThreshold,
        ControllerSettingsTriggerDeadzone,
        ControllerSettingsTriggerSensitivity,
        ControllerSettingsTriggerButton,
        
        // Shoulder Button Settings
        ControllerSettingsLeftSR,
        ControllerSettingsRightSR,
        ControllerSettingsLeftSL,
        ControllerSettingsRightSL,
        
        // Motion Control Settings
        ControllerSettingsMotion,
        ControllerSettingsMotionSensitivity,
        ControllerSettingsMotionDeadzone,
        ControllerSettingsMotionGyro,
        ControllerSettingsMotionGyroSensitivity,
        ControllerSettingsMotionGyroDeadzone,
        ControllerSettingsMotionAccelerometer,
        ControllerSettingsMotionUseCemuhookCompatibleMotion,
        ControllerSettingsMotionServerHost,
        ControllerSettingsMotionControllerSlot,
        ControllerSettingsMotionRightJoyConSlot,
        ControllerSettingsMotionMirrorInput,
        
        // Rumble Settings
        ControllerSettingsRumble,
        ControllerSettingsRumbleIntensity,
        ControllerSettingsRumbleStrength,
        ControllerSettingsRumbleStrongMultiplier,
        ControllerSettingsRumbleWeakMultiplier,

        // Player Settings
        ControllerSettingsPlayer,
        ControllerSettingsPlayerIndex,
        
        // Amiibo Dialogs
        DialogAmiiboApiTitle,
        DialogAmiiboApiConnectErrorMessage,
        
        // Updater Messages
        UpdaterExtracting,
        UpdaterRenaming,
        UpdaterRenameFailed,
        UpdaterAddingFiles,
        DialogUpdaterNoInternetMessage,
        DialogUpdaterNoInternetSubMessage,
        DialogUpdaterDirtyBuildMessage,
        DialogUpdaterDirtyBuildSubMessage,
        
        // Network
        NetworkInterfaceDefault,
        
        // Profile Dialogs
        DialogProfileInvalidProfileErrorMessage,
        DialogProfileDefaultProfileOverwriteErrorMessage,
        DialogProfileInvalidProfileNameErrorMessage,
        DialogProfileDeleteProfileTitle,
        DialogProfileDeleteProfileMessage,
        
        // Firmware Installer
        DialogFirmwareInstallerFirmwareNotFoundErrorMessage,
        DialogFirmwareInstallerFirmwareInstallTitle,
        DialogFirmwareInstallerFirmwareInstallMessage,
        DialogFirmwareInstallerFirmwareInstallSubMessage,
        DialogFirmwareInstallerFirmwareInstallConfirmMessage,
        DialogFirmwareInstallerFirmwareInstallWaitMessage,
        DialogFirmwareInstallerFirmwareInstallSuccessMessage,
        
        // Mod Manager Dialogs
        DialogModDeleteNoParentMessage,
        DialogModInvalidMessage,
        DialogModAlreadyExistsMessage,
        SelectModDialogTitle,
        
        // Amiibo Window
        Unknown,
        Usage,
        Writable,
        DialogAmiiboApiFailFetchMessage,
        
        // Controller Settings
        ControllerSettingsPlayer1,
        ControllerSettingsPlayer2,
        ControllerSettingsPlayer3,
        ControllerSettingsPlayer4,
        ControllerSettingsPlayer5,
        ControllerSettingsPlayer6,
        ControllerSettingsPlayer7,
        ControllerSettingsPlayer8,
        ControllerSettingsHandheld,
        ControllerSettingsControllerType,
        ControllerSettingsControllerTypeHandheld,
        ControllerSettingsControllerTypeProController,
        ControllerSettingsControllerTypeJoyConPair,
        ControllerSettingsControllerTypeJoyConLeft,
        ControllerSettingsControllerTypeJoyConRight,
        
        // Game List Headers
        GameListHeaderApplication,
        GameListHeaderDeveloper,
        GameListHeaderLastPlayed,
        GameListHeaderTimePlayed,
        GameListHeaderFileExtension,
        GameListHeaderFileSize,
        GameListHeaderPath,
        CommonFavorite,
        
        // System Modes
        Docked,
        Handheld,
        
        // Linux System Messages
        LinuxVmMaxMapCountWarningTextPrimary,
        LinuxVmMaxMapCountWarningTextSecondary,
        LinuxVmMaxMapCountDialogTitle,
        LinuxVmMaxMapCountDialogTextPrimary,
        LinuxVmMaxMapCountDialogTextSecondary,
        LinuxVmMaxMapCountDialogButtonUntilRestart,
        LinuxVmMaxMapCountDialogButtonPersistent,
        
        // Firmware Dialogs
        DialogFirmwareNoFirmwareInstalledMessage,
        DialogFirmwareInstallEmbeddedMessage,
        DialogFirmwareInstalledMessage,
        DialogFirmwareInstallEmbeddedSuccessMessage,
        
        // Content Dialogs
        DialogConfirmationTitle,
        DialogWarningTitle,
        DialogErrorMessage,
        DialogExitTitle,
        DialogExitMessage,
        DialogExitSubMessage,
        DialogStopEmulationTitle,
        DialogStopEmulationMessage,
        
        // DLC Manager
        SelectDlcDialogTitle,
        DialogDlcNoDlcErrorMessage,
        
        // NCA Extraction
        FolderDialogExtractTitle,
        DialogNcaExtractionTitle,
        DialogNcaExtractionMessage,
        DialogNcaExtractionMainNcaNotFoundErrorMessage,
        DialogNcaExtractionCheckLogErrorMessage,
        DialogNcaExtractionSuccessMessage,
        
        // Status
        Paused,
        
        // User Error Dialogs
        UserErrorUndefinedDescription,
        OpenSetupGuideMessage,
        DialogUserErrorDialogMessage,
        DialogUserErrorDialogInfoMessage,
        
        // Cache Management
        DialogPPTCDeletionErrorMessage,
        ShaderCachePurgeError,
        
        // Software Keyboard
        SoftwareKeyboardModeNumeric,
        SoftwareKeyboardModeAlphabet,
        SoftwareKeyboardModeASCII,
        SoftwareKeyboard,
        
        // Applet Dialogs
        DialogErrorAppletErrorExceptionMessage,
        
        // Settings
        DialogSettingsBackendThreadingWarningMessage,
        DialogSettingsBackendThreadingWarningTitle,
        SettingsButtonClose,
        
        // Window Headings
        DlcWindowHeading,
        ModWindowHeading,
        
        // Controller Settings
        ControllerRumbleTitle,
        ControllerMotionTitle,
        ControllerSettingsSave,
        ControllerSettingsClose,
        DialogControllerSettingsModifiedConfirmMessage,
        DialogControllerSettingsModifiedConfirmSubMessage,
        
        // User Profiles
        DialogUserProfileUnsavedChangesTitle,
        DialogUserProfileUnsavedChangesMessage,
        DialogUserProfileUnsavedChangesSubMessage,
        
        // Miscellaneous
        Never,
        NoUpdate,
        SettingsTabGeneralAdd,
        DlcManagerRemoveAllButton,
        SettingsButtonSave,
        SettingsTabGeneral,
        SettingsTabInput,
        SettingsTabHotkeys,
        SettingsTabSystem,
        SettingsTabCpu,
        SettingsTabGraphics,
        SettingsTabAudio,
        SettingsTabNetwork,
        SettingsTabLogging,
        SettingsButtonOk,
        SettingsButtonCancel,
        SettingsButtonApply,
        DlcManagerEnableAllButton,
        DlcManagerDisableAllButton,
        Search,
        ModManagerDeleteAllButton,
        BuildId,
        AmiiboSeriesLabel,
        AmiiboCharacterLabel,
        AmiiboOptionsShowAllLabel,
        AmiiboOptionsUsRandomTagLabel,
        AmiiboScanButtonLabel,
        AboutChangelogButton,
        AboutChangelogButtonTooltipMessage,
        AboutDisclaimerMessage,
        AboutAmiiboDisclaimerMessage,
        AboutPatreonUrlTooltipMessage,
        AboutGithubUrlTooltipMessage,
        AboutDiscordUrlTooltipMessage,
        AboutTwitterUrlTooltipMessage,
        AboutUrlTooltipMessage,
        AboutRyujinxAboutTitle,
        
        // User Errors
        UserErrorNoKeys,
        UserErrorNoFirmware,
        UserErrorFirmwareParsingFailed,
        UserErrorApplicationNotFound,
        UserErrorUnknown,
        UserErrorUndefined,
        UserErrorNoKeysDescription,
        UserErrorNoFirmwareDescription,
        UserErrorFirmwareParsingFailedDescription,
        UserErrorApplicationNotFoundDescription,
        UserErrorUnknownDescription,
        
        // Amiibo
        Amiibo,
        
        // User Profiles
        UserProfilesClose,
        DeleteUserSave,
        IrreversibleActionNote,
        
        // Updater
        RyujinxUpdaterMessage,
        UpdaterDownloading,
        
        // Software Keyboard
        SwkbdMinCharacters,
        SwkbdMinRangeCharacters,
        
        // Input
        AllKeyboards,
        
        // Connection
        ConnectionError,
        ApiError,
        
        // Controller Applet
        ControllerAppletDescription,
        ControllerAppletDocked,
        ControllerAppletControllers,
        ControllerAppletPlayer,
        ControllerAppletPlayers,
        ControllerAppletDisconnect,
        ControllerAppletConnect,
        
        // Game List Context Menu
        GameListContextMenuRunApplication,
        GameListContextMenuToggleFavorite,
        GameListContextMenuToggleFavoriteToolTip,
        GameListContextMenuCreateShortcut,
        GameListContextMenuCreateShortcutToolTip,
        GameListContextMenuCreateShortcutToolTipMacOS,
        GameListContextMenuOpenUserSaveDirectory,
        GameListContextMenuOpenUserSaveDirectoryToolTip,
        GameListContextMenuOpenDeviceSaveDirectory,
        GameListContextMenuOpenDeviceSaveDirectoryToolTip,
        GameListContextMenuOpenBcatSaveDirectory,
        GameListContextMenuOpenBcatSaveDirectoryToolTip,
        GameListContextMenuManageTitleUpdates,
        GameListContextMenuManageTitleUpdatesToolTip,
        GameListContextMenuManageDlc,
        GameListContextMenuManageDlcToolTip,
        GameListContextMenuManageCheat,
        GameListContextMenuManageCheatToolTip,
        GameListContextMenuManageMod,
        GameListContextMenuManageModToolTip,
        GameListContextMenuOpenModsDirectory,
        GameListContextMenuOpenModsDirectoryToolTip,
        GameListContextMenuOpenSdModsDirectory,
        GameListContextMenuOpenSdModsDirectoryToolTip,
        GameListContextMenuCacheManagement,
        GameListContextMenuCacheManagementPurgePptc,
        GameListContextMenuCacheManagementPurgePptcToolTip,
        GameListContextMenuCacheManagementPurgeShaderCache,
        GameListContextMenuCacheManagementPurgeShaderCacheToolTip,
        GameListContextMenuCacheManagementOpenPptcDirectory,
        GameListContextMenuCacheManagementOpenPptcDirectoryToolTip,
        GameListContextMenuCacheManagementOpenShaderCacheDirectory,
        GameListContextMenuCacheManagementOpenShaderCacheDirectoryToolTip,
        GameListContextMenuExtractData,
        GameListContextMenuExtractDataExeFS,
        GameListContextMenuExtractDataExeFSToolTip,
        GameListContextMenuExtractDataRomFS,
        GameListContextMenuExtractDataRomFSToolTip,
        GameListContextMenuExtractDataLogo,
        GameListContextMenuExtractDataLogoToolTip,
        
        // Input Device Settings
        ControllerSettingsInputDevice,
        ControllerSettingsInputDeviceType,
        
        // Profile Settings
        ControllerSettingsProfile,
        ControllerSettingsProfileNew,
        ControllerSettingsProfileDelete,
        ControllerSettingsLoadProfileToolTip,
        ControllerSettingsSaveProfileToolTip,
        ControllerSettingsDeleteProfileToolTip,
        ControllerSettingsRemoveProfileToolTip,
        
        // Button Settings
        ControllerSettingsButtons,
        ControllerSettingsButtonA,
        ControllerSettingsButtonB,
        ControllerSettingsButtonX,
        ControllerSettingsButtonY,
        
        // Configuration Buttons
        ControllerSettingsConfigureGeneral,
        ControllerSettingsConfigureMotion,
        ControllerSettingsConfigureVibration,
        ControllerSettingsConfigureButtons,
    }
}
