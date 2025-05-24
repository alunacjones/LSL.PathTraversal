using System.IO;

namespace LSL.PathTraversal;

/// <summary>
/// DirectoryInfo Extensions
/// </summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// Helper method to check for a file's existence in the given <see cref="DirectoryInfo"/>
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool FileExists(this DirectoryInfo directoryInfo, string fileName) =>
        File.Exists(Path.Combine(directoryInfo.GuardAgainstNull(nameof(directoryInfo)).FullName, fileName));
}