using Ryujinx.Common.Configuration;
using Ryujinx.Common.Utilities;
using System;
using System.IO;

namespace Ryujinx.UI.Common.Helper
{
    public static class ScreenshotHelper
    {
        /// <summary>
        /// The folder screenshots are saved to: <c>screenshots</c> next to the data in portable/custom mode,
        /// otherwise <c>Pictures/Ryujinx</c>.
        /// </summary>
        public static string GetDirectory() => AppDataManager.Mode switch
        {
            AppDataManager.LaunchMode.Portable or AppDataManager.LaunchMode.Custom => Path.Combine(AppDataManager.BaseDirPath, "screenshots"),
            _ => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Ryujinx"),
        };

        public static string BuildFileName(string applicationName, DateTime now) =>
            $"{FileSystemUtils.SanitizeFileName(applicationName)}_{now.Year}-{now.Month:D2}-{now.Day:D2}_{now.Hour:D2}-{now.Minute:D2}-{now.Second:D2}.png";
    }
}
